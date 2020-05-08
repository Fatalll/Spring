using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Resolve;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Plugins.Spring
{
    public class SpringReference : TreeReferenceBase<SpringReferenceNodeElement>
    {
        public SpringReference([NotNull] SpringReferenceNodeElement owner) : base(owner)
        {
        }

        public override ResolveResultWithInfo ResolveWithoutCache()
        {
            var file = myOwner.GetContainingFile();
            if (file != null)
            {
                IDeclaration result = null;
                foreach (var descendant in file.Descendants())
                {
                    if (descendant == myOwner) break;
                    if (descendant is IDeclaration declaration && declaration.DeclaredName == GetName())
                    {
                        result = declaration;
                    }
                }

                if (result != null) return new ResolveResultWithInfo(new SimpleResolveResult(result.DeclaredElement), ResolveErrorType.OK);
            }
            
            return ResolveResultWithInfo.Unresolved;
        }

        public override string GetName() => myOwner.GetNodeText;

        public override ISymbolTable GetReferenceSymbolTable(bool useReferenceName) =>
            throw new System.NotImplementedException();

        public override TreeTextRange GetTreeTextRange() => new TreeTextRange(myOwner.GetTreeStartOffset(), myOwner.GetTreeStartOffset() + GetName().Length);

        public override IReference BindTo(IDeclaredElement element) => this;

        public override IReference BindTo(IDeclaredElement element, ISubstitution substitution) => this;

        public override IAccessContext GetAccessContext() => new DefaultAccessContext(myOwner);
        
        public override bool IsValid() => myOwner.IsValid();
    }
}