﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        public string DisplayText => "Transform into request handler";

        public object IconMoniker => "IconMoniker";

        public string IconAutomationText => "IconAutomationText";

        public string InputGestureText => "InputGestureText";

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
            var generator = new MemberedTypeGenerator();

            string requestCode = generator.GenerateSyntax(this.requestHandler.RequestType);
            this.workspaceManager.CreateOrUpdateDocument(this.requestHandler.RequestType.Name, requestCode);

            string responseCode = generator.GenerateSyntax(this.requestHandler.ResponseType);
            this.workspaceManager.CreateOrUpdateDocument(this.requestHandler.ResponseType.Name, responseCode);
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = Guid.NewGuid();
            return true;
        }
    }
}