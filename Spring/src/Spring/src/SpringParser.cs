using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using ICSharpCode.NRefactory.CSharp;
using JetBrains.Application.DataContext;
using JetBrains.Application.Settings;
using JetBrains.Core;
using JetBrains.Diagnostics;
using JetBrains.DocumentModel;
using JetBrains.Lifetimes;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.CSharp.Errors;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.ReSharper.Feature.Services.Occurrences;
using JetBrains.ReSharper.Feature.Services.SelectEmbracingConstruct;
using JetBrains.ReSharper.Features.Navigation.Features.GoToDeclaration;
using JetBrains.ReSharper.I18n.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.TreeBuilder;
using JetBrains.Text;
using JetBrains.Util;
using JetBrains.Util.Logging;

namespace JetBrains.ReSharper.Plugins.Spring
{
    internal class SpringParser : IParser
    {
        private readonly ILexer myLexer;

        public SpringParser(ILexer lexer)
        {
            myLexer = lexer;
        }

        public IFile ParseFile()
        {
            using (var def = Lifetime.Define())
            {
                var builder = new PsiBuilder(myLexer, SpringFileNodeType.Instance, new TokenFactory(), def.Lifetime);
                var fileMark = builder.Mark();
                
                var parser = new CParser(new CommonTokenStream(new CLexer(new AntlrInputStream(myLexer.Buffer.GetText()))));
                parser.AddErrorListener(new ErrorListener(builder));
                new Visitor(builder).Visit(parser.compilationUnit());

                builder.Done(fileMark, SpringFileNodeType.Instance, null);
                var file = (IFile)builder.BuildTree();
                return file;
            }
        }
        
        private class Visitor : CBaseVisitor<Unit>
        {
            private readonly PsiBuilder _builder;

            public Visitor(PsiBuilder builder)
            {
                _builder = builder;
            }

            public override Unit VisitPrimaryExpression(CParser.PrimaryExpressionContext context)
            {
                if (context.Identifier() != null)
                {
                    _builder.ResetCurrentLexeme(context.SourceInterval.a, context.SourceInterval.a);
                    var mark = _builder.Mark();
                    base.VisitChildren(context);
                    _builder.Done(mark, SpringReferenceNodeElement.NODE_TYPE, context);
                }
                else
                {
                    VisitChildren(context);
                }
                
                return Unit.Instance;
            }

            public override Unit VisitDirectDeclarator(CParser.DirectDeclaratorContext context)
            { 
                if (context.Identifier() != null)
                {
                    _builder.ResetCurrentLexeme(context.SourceInterval.a, context.SourceInterval.a);
                    var mark = _builder.Mark();
                    base.VisitChildren(context);
                    _builder.Done(mark, SpringDeclarationNodeElement.NODE_TYPE, context);
                }
                else
                {
                    VisitChildren(context);
                }
                
                return Unit.Instance;
            }

            public override Unit VisitChildren(IRuleNode node)
            {
                _builder.ResetCurrentLexeme(node.SourceInterval.a, node.SourceInterval.a);
                var mark = _builder.Mark();
                base.VisitChildren(node);
                _builder.Done(mark, SpringNodeElement.NODE_TYPE, node.RuleContext);
                return Unit.Instance;
            }
        }
        

        class ErrorListener : BaseErrorListener
        {
            private readonly PsiBuilder _builder;

            public ErrorListener(PsiBuilder builder)
            {
                _builder = builder;
            }

            public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, 
                int line, int charPositionInLine, string msg, RecognitionException e)
            {
                _builder.ResetCurrentLexeme(offendingSymbol.TokenIndex, offendingSymbol.TokenIndex);
                _builder.DoneBeforeWhitespaces(_builder.Mark(), SpringPsiBuilderErrorElement.NODE_TYPE, new ErrorMsgWithLengthAndType(msg, offendingSymbol.Text.Length));
            }
        }
    }

    [DaemonStage]
    class SpringDaemonStage : DaemonStageBase<SpringFile>
    {
        protected override IDaemonStageProcess CreateDaemonProcess(IDaemonProcess process, DaemonProcessKind processKind, SpringFile file,
            IContextBoundSettingsStore settingsStore)
        {
            return new SpringDaemonProcess(process, file);
        }

        internal class SpringDaemonProcess : IDaemonStageProcess
        {
            private readonly SpringFile myFile;
            private readonly SpringReferenceFactory _factory;

            public SpringDaemonProcess(IDaemonProcess process, SpringFile file)
            {
                myFile = file;
                DaemonProcess = process;
                _factory = new SpringReferenceFactory();
            }

            public void Execute(Action<DaemonStageResult> committer)
            {
                var highlightings = new List<HighlightingInfo>();
                foreach (var treeNode in myFile.Descendants())
                {
                    
                    if (treeNode is SpringPsiBuilderErrorElement error)
                    {
                        var range = error.GetDocumentRange().ExtendRight(error.Length);

                        highlightings.Add(error.IsSyntaxError
                            ? new HighlightingInfo(range, new CSharpSyntaxError(error.ErrorDescription, range))
                            : new HighlightingInfo(range, new SpringUnresolvedError(error.ErrorDescription, range)));
                    }
                    else
                    {
                        foreach (var reference in _factory.GetReferences(treeNode, ReferenceCollection.Empty))
                        {
                            if (reference.Resolve().Info.ResolveErrorType != ResolveErrorType.OK)
                            {
                                var range = reference.GetDocumentRange();
                                highlightings.Add(new HighlightingInfo(range, new SpringUnresolvedError("Can't resolve identifier", range)));
                            }
                        }
                    }
                }
                
                var result = new DaemonStageResult(highlightings);
                committer(result);
            }

            public IDaemonProcess DaemonProcess { get; }
        }

        protected override IEnumerable<SpringFile> GetPsiFiles(IPsiSourceFile sourceFile)
        {
            yield return (SpringFile)sourceFile.GetDominantPsiFile<SpringLanguage>();
        }
    } 

    internal class TokenFactory : IPsiBuilderTokenFactory
    {
        public LeafElementBase CreateToken(TokenNodeType tokenNodeType, IBuffer buffer, int startOffset, int endOffset)
        {
            return tokenNodeType.Create(buffer, new TreeOffset(startOffset), new TreeOffset(endOffset));
        }
    }
    

    [ProjectFileType(typeof (SpringProjectFileType))]
    public class SelectEmbracingConstructProvider : ISelectEmbracingConstructProvider
    {
        public bool IsAvailable(IPsiSourceFile sourceFile)
        {
            return sourceFile.LanguageType.Is<SpringProjectFileType>();
        }

        public ISelectedRange GetSelectedRange(IPsiSourceFile sourceFile, DocumentRange documentRange)
        {
            var file = (SpringFile) sourceFile.GetDominantPsiFile<SpringLanguage>();
            var node = file.FindNodeAt(documentRange);
            return new SpringTreeNodeSelection(file, node);
        }

        public class SpringTreeNodeSelection : TreeNodeSelection<SpringFile>
        {
            public SpringTreeNodeSelection(SpringFile fileNode, ITreeNode node) : base(fileNode, node)
            {
            }

            public override ISelectedRange Parent => new SpringTreeNodeSelection(FileNode, TreeNode.Parent);
        }
    }
}
