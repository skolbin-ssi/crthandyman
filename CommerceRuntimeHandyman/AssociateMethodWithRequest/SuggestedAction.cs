﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;

namespace CommerceRuntimeHandyman.AssociateMethodWithRequest
{
    public class SuggestedAction : ISuggestedAction
    {
        public bool HasActionSets => false;

        public string DisplayText { get; set; }

        public object IconMoniker => "IconMoniker";

        public string IconAutomationText => "IconAutomationText";

        public string InputGestureText => "InputGestureText";

        public bool HasPreview => false;

        ImageMoniker ISuggestedAction.IconMoniker => default(ImageMoniker);

        public SuggestedAction(string methodName)
        {
            this.DisplayText = $"I found {methodName}";
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
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = Guid.NewGuid();
            return true;
        }
    }
}
