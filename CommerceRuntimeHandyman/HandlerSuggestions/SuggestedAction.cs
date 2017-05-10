using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Handyman.Generators;
using Handyman.Types;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;

namespace CommerceRuntimeHandyman.AssociateMethodWithRequest
{
    public class SuggestedAction : ISuggestedAction
    {
        public bool HasActionSets => false;

        public string DisplayText => "Create or update request/response";

        public object IconMoniker => string.Empty;

        public string IconAutomationText => string.Empty;

        public string InputGestureText => string.Empty;

        public bool HasPreview => false;

        ImageMoniker ISuggestedAction.IconMoniker => default(ImageMoniker);

        private RequestHandlerDefinition requestHandler;
        private WorkspaceManager workspaceManager;

        public SuggestedAction(WorkspaceManager workspaceManager, RequestHandlerDefinition requestHandler)
        {
            this.requestHandler = requestHandler;
            this.workspaceManager = workspaceManager;
        }

        public void Dispose()
        {
        }

        public Task<IEnumerable<SuggestedActionSet>> GetActionSetsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<SuggestedActionSet>>(null);
        }

        public Task<object> GetPreviewAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }

        public void Invoke(CancellationToken cancellationToken)
        {
            if (!this.workspaceManager.CreateOrUpdateRequestHandlerDefinition(this.requestHandler))
            {
                MessageBox.Show("Couldn't apply changes to project");
            }
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = Guid.NewGuid();
            return true;
        }
    }
}