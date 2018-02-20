using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.FindSymbols;
using Handyman.Types;

namespace Handyman.DocumentAnalyzers
{
    /// <summary>
    /// Analyzes syntax tree and tries to find a reference to a request or response.
    /// </summary>
    public sealed class RequestResponseTypeAnalyzer
    {
        private readonly AnalysisContext context;

        public RequestResponseTypeAnalyzer(AnalysisContext context)
        {
            this.context = context;
        }

        public async Task<RequestHandlerDefinition> FindImplementation(int tokenPosition, CancellationToken cancellationToken = default(CancellationToken))
        {
            // TODO: handle class definition symbol
            SyntaxNode node = this.context.SyntaxRoot.FindToken(tokenPosition).Parent;

            // not always the identifier name syntax node will result in the type info directly (e.g. on a constructor statement, you need the constructor statement itself)
            // I haven't found out a deterministic way to do this, but it seems intuitive that the type information is not 'too far away' from the identifier
            TypeInfo info;
            for (int i = 0;
                i < 3 && node != null && (info = this.context.SemanticModel.GetTypeInfo(node, cancellationToken)).Type == null;
                node = node.Parent, i++) ;

            if (info.Type != null)
            {
                bool isRequest = info.Type.IsDerivedFrom(this.context.CommerceRuntimeReference.RequestTypeSymbol);
                bool isResponse = info.Type.IsDerivedFrom(this.context.CommerceRuntimeReference.ResponseTypeSymbol);

                if (isRequest || isResponse)
                {
                    var locations = (await SymbolFinder.FindReferencesAsync(info.Type, context.Document.Project.Solution, cancellationToken))
                        .First(r => r.Definition == info.Type).Locations;

                    // for each location, analyze the code and see if it is a request handler
                    var requestHandlersTasks = locations.Select(async l =>
                        {
                            var context = await AnalysisContext.Create(l.Document, cancellationToken);
                            var analyzer = new RequestHandlerAnalyzer(context);
                            return analyzer.TryGetRequestHandlerFromSyntaxTree(l.Location.SourceSpan, cancellationToken);                        
                        })
                        .ToArray();

                    await Task.WhenAll(requestHandlersTasks);

                    // filter out results that are not request handlers
                    var requestHandlers = requestHandlersTasks.Select(r => r.Result).Where(r => r != null);

                    // for each request handler found, see if it implements the request we have
                    var requestHandler = requestHandlers.FirstOrDefault(h => h.DeclaredSupportedRequestTypes.Contains(info.Type));

                    if (requestHandler != null)
                    {
                        return requestHandler;
                    }
                }
            }

            return null;
        }
    }
}