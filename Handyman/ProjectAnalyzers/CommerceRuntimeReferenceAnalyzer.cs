﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Handyman.Errors;
using Handyman.Types;
using Microsoft.CodeAnalysis;

namespace Handyman.ProjectAnalyzers
{
    /// <summary>
    /// Finds references to commerce runtime.
    /// </summary>
    public sealed class CommerceRuntimeReferenceAnalyzer
    {
        ////private static CommerceRuntimeReference CachedReference = null;

        private readonly Project project;
        private Compilation compilation;

        public CommerceRuntimeReferenceAnalyzer(Project project)
        {
            this.project = project ?? throw new ArgumentNullException(nameof(project));
        }

        /// <summary>
        /// Tries to find a reference for the commerce runtime.
        /// </summary>
        /// <returns>The commerce runtime reference.</returns>
        public async Task<CommerceRuntimeReference> Find(CancellationToken cancellationToken = default)
        {
            // TODO: it seems that the types found on different compilations (despite being idential) do not implement Equality
            // this is, CommerceRuntimeReference.RequestTypeSymbol.Equals(a request symbol from another compilation) -> false
            // this affects some flows - need to review what is the best approach to do this
            ////if (CachedReference != null)
            ////{
            ////    return CachedReference;
            ////}

            this.compilation = await this.project.GetCompilationAsync(cancellationToken);
            var requestType = this.compilation.GetTypeByMetadataName("Microsoft.Dynamics.Commerce.Runtime.Messages.Request");
            var responseType = this.compilation.GetTypeByMetadataName("Microsoft.Dynamics.Commerce.Runtime.Messages.Response");
            var requestHandlerInterfaceType = this.compilation.GetTypeByMetadataName("Microsoft.Dynamics.Commerce.Runtime.IRequestHandler");

            if (requestHandlerInterfaceType == null || requestType == null || responseType == null)
            {
                throw new HandymanErrorException(new Error("CannotResolveCommerceRuntimeReference", "A reference to the CommerceRuntime couldn't be found. Please make sure the CommerceRuntime is referenced on the project and there are no compilation errors."));
            }

            string _namespace = requestType.ContainingNamespace.ToString();

            var reference = new CommerceRuntimeReference()
            {
                RequestBaseClassFqn = $"{_namespace}.Request",
                ResponseBaseClassFqn = $"{_namespace}.Response",
                RequestTypeSymbol = requestType,
                ResponseTypeSymbol = responseType,
                IRequestHandlerTypeSymbol = requestHandlerInterfaceType,
                VoidResponse = new ResponseType("NullResponse", new Member[0], string.Empty, _namespace)
                {
                    IsVoidResponse = true,
                    Namespace = _namespace
                }
            };

            ////if (requestType != null)
            ////{
            ////    // cache if we found the assembly
            ////    CachedReference = reference;
            ////}

            return reference;
        }

        ////private GetRequestNamespace(Project project)
        ////{
        ////    var c = await project.GetCompilationAsync();
        ////    if (c.GetTypeByMetadataName("Microsoft.Dynamics.Commerce.Runtime.Messages.Request") != null)
        ////    {
        ////        return "Microsoft.Dynamics.Commerce.Runtime.Messages";
        ////    }

        ////    return null;
        ////}
    }
}
