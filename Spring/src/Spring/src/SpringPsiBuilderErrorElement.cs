using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.TreeBuilder;

namespace JetBrains.ReSharper.Plugins.Spring
{
    public class SpringPsiBuilderErrorElement : CompositeElement, IErrorElement, ITreeNode
    {
        public static readonly CompositeNodeWithArgumentType NODE_TYPE = new SpringErrorElementNodeType();
        private ErrorMsgWithLengthAndType _errorMsgWithLengthAndType;

        public SpringPsiBuilderErrorElement(ErrorMsgWithLengthAndType errorMsgWithLengthAndType)
        {
            _errorMsgWithLengthAndType = errorMsgWithLengthAndType;
        }
        
        public override PsiLanguageType Language => this.LanguageFromParent;

        public override NodeType NodeType => NODE_TYPE;
        
        public string ErrorDescription => _errorMsgWithLengthAndType.Msg;

        public int Length => _errorMsgWithLengthAndType.Length;

        public bool IsSyntaxError => _errorMsgWithLengthAndType.Syntax;

        public override bool IsFiltered() => true;
    }

    public class ErrorMsgWithLengthAndType
    {
        public ErrorMsgWithLengthAndType(string msg, int length, bool isSyntax = true)
        {
            Msg = msg;
            Length = length;
            Syntax = isSyntax;
        }

        public string Msg;
        public int Length;
        public bool Syntax;
    }

    class SpringErrorElementNodeType : CompositeNodeWithArgumentType
    {
        public SpringErrorElementNodeType()
            : base("ERROR_ELEMENT", 4)
        {
        }

        public override CompositeElement Create(object messageWithLength)
        {
            return new SpringPsiBuilderErrorElement((ErrorMsgWithLengthAndType) messageWithLength);
        }

        public override CompositeElement Create()
        {
            Assertion.Fail("Can't create PsiBuilderErrorElement without error message and length");
            return new PsiBuilderErrorElement("Syntax error");
        }
    }
}