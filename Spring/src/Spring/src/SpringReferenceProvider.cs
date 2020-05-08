using System.Collections.Generic;
using JetBrains.DataFlow;
using JetBrains.Lifetimes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;

namespace JetBrains.ReSharper.Plugins.Spring
{
    [ReferenceProviderFactory]
    public class SpringReferenceProvider : IReferenceProviderFactory
    {
        public SpringReferenceProvider(Lifetime lifetime)
        {
            Changed = new Signal<IReferenceProviderFactory>(lifetime, "SPRING_REF_PROVIDER");
        }
        
        public IReferenceFactory CreateFactory(IPsiSourceFile sourceFile, IFile file, IWordIndex wordIndexForChecks)
        {
            return Equals(sourceFile.PrimaryPsiLanguage, SpringLanguage.Instance) ? new SpringReferenceFactory() : null;
        }

        public ISignal<IReferenceProviderFactory> Changed { get; }
    }

    public class SpringReferenceFactory : IReferenceFactory
    {
        public ReferenceCollection GetReferences(ITreeNode element, ReferenceCollection oldReferences)
        {
            if (element is SpringReferenceNodeElement reference)
            {
                return new ReferenceCollection(new List<IReference> { new SpringReference(reference) });
            }

            return ReferenceCollection.Empty;
        }

        public bool HasReference(ITreeNode element, IReferenceNameContainer names)
        {
            return element is SpringReferenceNodeElement reference && names.Contains(reference.GetText());
        }
    }
}