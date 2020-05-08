using System.Runtime.Remoting.Contexts;
using System.Xml;
using Antlr4.Runtime;
using JetBrains.Diagnostics;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.TreeBuilder;
using Mono.CSharp;

namespace JetBrains.ReSharper.Plugins.Spring
{
    public class SpringNodeElement : CompositeElement
    {
        public static readonly CompositeNodeWithArgumentType NODE_TYPE = new SpringNodeElementType();

        protected RuleContext _context;

        public SpringNodeElement(RuleContext context)
        {
            _context = context;
        }

        public RuleContext Context => _context;

        public string GetNodeText => _context.GetText();

        public override NodeType NodeType => NODE_TYPE;
        public override PsiLanguageType Language => this.LanguageFromParent;
    }

    public class SpringDeclarationNodeElement : SpringNodeElement, IDeclaration
    {
        public new static readonly CompositeNodeWithArgumentType NODE_TYPE = new SpringDeclarationNodeElementType();
        public SpringDeclarationNodeElement(RuleContext context) : base(context)
        {
            DeclaredElement = new SpringDeclaredElement(this);
        }

        public XmlNode GetXMLDoc(bool inherit) => null;

        public void SetName(string name) { }

        public TreeTextRange GetNameRange() => new TreeTextRange(GetTreeStartOffset(), GetTreeStartOffset() + GetNodeText.Length);

        public bool IsSynthetic() => false;

        public IDeclaredElement DeclaredElement { get; }
        public string DeclaredName => GetNodeText;
    }
    
    public class SpringReferenceNodeElement : SpringNodeElement
    {
        public new static readonly CompositeNodeWithArgumentType NODE_TYPE = new SpringReferenceNodeElementType();

        public SpringReferenceNodeElement(RuleContext context) : base(context)
        {
            
        }

        public override NodeType NodeType => NODE_TYPE;
    }
    
    class SpringNodeElementType : CompositeNodeWithArgumentType
    {
        public SpringNodeElementType()
            : base("SPRING_NODE", 5)
        {
        }

        public override CompositeElement Create(object context)
        {
            return new SpringNodeElement((RuleContext) context);
        }

        public override CompositeElement Create()
        {
            Assertion.Fail("Can't create SpringNodeElementType without rule context");
            return new PsiBuilderErrorElement("Syntax error");
        }
    }

    class SpringDeclarationNodeElementType : CompositeNodeWithArgumentType
    {
        public SpringDeclarationNodeElementType()
            : base("SPRING_NODE_DECLARATION", 6)
        {
        }

        public override CompositeElement Create(object context)
        {
            return new SpringDeclarationNodeElement((RuleContext) context);
        }

        public override CompositeElement Create()
        {
            Assertion.Fail("Can't create SpringDeclarationNodeElementType without rule context");
            return new PsiBuilderErrorElement("Syntax error");
        }
    }
    
    class SpringReferenceNodeElementType : CompositeNodeWithArgumentType
    {
        public SpringReferenceNodeElementType()
            : base("SPRING_NODE_REFERENCE", 7)
        {
        }

        public override CompositeElement Create(object context)
        {
            return new SpringReferenceNodeElement((RuleContext) context);
        }

        public override CompositeElement Create()
        {
            Assertion.Fail("Can't create SpringReferenceNodeElementType without rule context");
            return new PsiBuilderErrorElement("Syntax error");
        }
    }
}