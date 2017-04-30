using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Handyman.Analyzer;
using Handyman.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace CommerceRuntimeHandyman.AssociateMethodWithRequest
{
    public class SuggestedActionsSource : ISuggestedActionsSource
    {
        private WorkspaceManager workspaceManager;
        public event EventHandler<EventArgs> SuggestedActionsChanged;

        public SuggestedActionsSource(WorkspaceManager workspaceManager)
        {
            this.workspaceManager = workspaceManager;
        }

        public void Dispose()
        {
            SuggestedActionsChanged = null;
        }

        public IEnumerable<SuggestedActionSet> GetSuggestedActions(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken)
        {
            var handlerImplementation = TryGetMethodDefinition(range, cancellationToken).Result;

            if (handlerImplementation != null)
            {
                yield return new SuggestedActionSet(new[] { new SuggestedAction(this.workspaceManager, handlerImplementation) });
            }
        }

        public async Task<bool> HasSuggestedActionsAsync(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken)
        {
            return await TryGetMethodDefinition(range, cancellationToken) != null;
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = Guid.NewGuid();
            return true;
        }

        private async Task<RequestHandlerDefinition> TryGetMethodDefinition(SnapshotSpan range, CancellationToken cancellationToken)
        {
            var document = range.Snapshot.TextBuffer.GetRelatedDocuments().FirstOrDefault();

            if (document != null)
            {
                return await new RequestHandlerAnalyzer(document).TryGetHandlerDefinition(range.Start, cancellationToken);
            }

            return null;
        }
    }
}
