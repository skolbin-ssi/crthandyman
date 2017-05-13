using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Handyman.Analyzers
{
    /// <summary>
    /// A class that groups together the document, the syntax tree root and the semantic model.
    /// </summary>
    public class AnalysisContext
    {
        public AnalysisContext(Document document, SemanticModel semanticModel, SyntaxNode syntaxRoot)
        {
            this.Document = document;
            this.SyntaxRoot = syntaxRoot;
            this.SemanticModel = semanticModel;
        }

        public Document Document { get; private set; }

        public SyntaxNode SyntaxRoot { get; private set; }

        public SemanticModel SemanticModel { get; private set; }

        public static async Task<AnalysisContext> Create(Document document, CancellationToken cancellationToken)
        {
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var syntaxRoot = await semanticModel.SyntaxTree.GetRootAsync(cancellationToken);

            return new AnalysisContext(document, semanticModel, syntaxRoot);
        }
    }
}
