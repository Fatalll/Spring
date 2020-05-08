using System.Collections;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.NRefactory;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util.DataStructures;

namespace JetBrains.ReSharper.Plugins.Spring
{
    public class SpringDeclaredElement : IDeclaredElement
    {
        private readonly IDeclaration _declaration;
        public SpringDeclaredElement(IDeclaration declaration)
        {
            _declaration = declaration;
        }

        public IPsiServices GetPsiServices() => _declaration.GetPsiServices();

        public IList<IDeclaration> GetDeclarations() => new List<IDeclaration> { _declaration };

        public IList<IDeclaration> GetDeclarationsIn(IPsiSourceFile sourceFile)
        {
            return _declaration.GetSourceFile() == sourceFile ? GetDeclarations() : EmptyList<IDeclaration>.Instance;
        }

        public DeclaredElementType GetElementType() => CLRDeclaredElementType.FIELD;

        public XmlNode GetXMLDoc(bool inherit) => _declaration.GetXMLDoc(inherit);

        public XmlNode GetXMLDescriptionSummary(bool inherit) => null;

        public bool IsValid() => _declaration.IsValid();

        public bool IsSynthetic() => _declaration.IsSynthetic();

        public HybridCollection<IPsiSourceFile> GetSourceFiles()
        {
            return _declaration.GetSourceFile() == null
                ? HybridCollection<IPsiSourceFile>.Empty
                : new HybridCollection<IPsiSourceFile>(_declaration.GetSourceFile());
        }

        public bool HasDeclarationsIn(IPsiSourceFile sourceFile) => _declaration.GetSourceFile() == sourceFile;

        public string ShortName => _declaration.DeclaredName;
        public bool CaseSensitiveName => true;
        public PsiLanguageType PresentationLanguage => _declaration.Language;
    }
}