using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon.Impl;
using JetBrains.ReSharper.Feature.Services.Daemon;

namespace JetBrains.ReSharper.Plugins.Spring
{
    [StaticSeverityHighlighting(Severity.ERROR, "SpringErrors", OverlapResolve = OverlapResolveKind.UNRESOLVED_ERROR)]
    public class SpringUnresolvedError : SyntaxErrorBase, IHighlightingWithNavigationOffset, IHighlighting
    {
        public int NavigationOffset { get; }

        public SpringUnresolvedError(string toolTip, DocumentRange range, int navigationOffset = 0)
            : base(toolTip, range)
        {
            this.NavigationOffset = navigationOffset;
        }
    }
}