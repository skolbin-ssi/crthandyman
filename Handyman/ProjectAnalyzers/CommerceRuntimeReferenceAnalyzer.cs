using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Handyman.Types;
using Microsoft.CodeAnalysis;

namespace Handyman.ProjectAnalyzers
{
    /// <summary>
    /// Finds references to commerce runtime.
    /// </summary>
    public sealed class CommerceRuntimeReferenceAnalyzer
    {
        private static CommerceRuntimeReference CachedReference = null;

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
        public async Task<CommerceRuntimeReference> Find(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (CachedReference != null)
            {
                return CachedReference;
            }

            this.compilation = await this.project.GetCompilationAsync(cancellationToken);
            var requestType = this.compilation.GetTypeByMetadataName("Microsoft.Dynamics.Commerce.Runtime.Messages.Request");

            string _namespace = requestType?.ContainingNamespace.ToString();

            var reference = new CommerceRuntimeReference()
            {
                RequestBaseClassFqn = _namespace != null ? $"{_namespace}.Request" : Settings.SettingsManager.Instance.RequestInterfaceFQN ?? string.Empty,
                ResponseBaseClassFqn = _namespace != null ? $"{_namespace}.Response" : Settings.SettingsManager.Instance.ResponseInterfaceFQN ?? string.Empty,
                VoidResponse = new ResponseType("NullResponse", new Member[0], string.Empty, _namespace)
                {
                    IsVoidResponse = true
                }
            };

            if (requestType != null)
            {
                // cache if we found the assembly
                CachedReference = reference;
            }

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
