﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommerceRuntimeHandyman.Types;
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
        public event EventHandler<EventArgs> SuggestedActionsChanged;

        public void Dispose()
        {
            SuggestedActionsChanged = null;
        }

        public IEnumerable<SuggestedActionSet> GetSuggestedActions(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken)
        {
            var symbol = TryGetMethodDefinition(range, cancellationToken).Result;

            if (symbol != null)
            {
                yield return new SuggestedActionSet(new[] { new SuggestedAction(symbol.Name) });
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

        private async Task<IMethodSymbol> TryGetMethodDefinition(SnapshotSpan range, CancellationToken cancellationToken)
        {
            var document = range.Snapshot.TextBuffer.GetRelatedDocuments().FirstOrDefault();

            if (document != null)
            {
                var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
                var syntaxTree = await semanticModel.SyntaxTree.GetRootAsync(cancellationToken);
                var token = syntaxTree.FindToken(range.Start);

                foreach (var node in token.Parent.AncestorsAndSelf())
                {
                    Type nodeType = node.GetType();
                    if (nodeType == typeof(MethodDeclarationSyntax))
                    {
                        return (IMethodSymbol)semanticModel.GetDeclaredSymbol(node);
                    }
                    else if (nodeType == typeof(BlockSyntax))
                    {
                        return null;
                    }
                }
            }

            return null;
        }
    }
}