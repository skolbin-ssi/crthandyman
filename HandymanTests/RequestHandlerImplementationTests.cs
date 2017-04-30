using System.Linq;
using System.Threading.Tasks;
using Handyman.Tests.TestHelpers;
using Handyman.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Handyman.Tests
{
    [TestClass]
    public class RequestHandlerImplementationTests
    {
        [TestMethod]
        public void TryParse_ValidMethod()
        {
            var code = @"
            class HandlerTest
            {
                public TestResponse DoWork(TestRequest request)
                {
                    return null;
                }
            }";

            SyntaxTree tree;
            var compilation = RoslynHelper.Compile(code, out tree);
            var semanticModel = compilation.GetSemanticModel(tree);

            var methodDeclaration = tree.GetRoot()
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .First();

            var methodSymbol = (IMethodSymbol)semanticModel.GetDeclaredSymbol(methodDeclaration);

            Assert.IsNotNull(RequestHandlerDefinition.TryParse(methodSymbol));
        }
    }
}
