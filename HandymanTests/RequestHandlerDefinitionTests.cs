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
    public class RequestHandlerDefinitionTests
    {
        [TestMethod]
        public void TryParse_ValidMethod()
        {
            var code = @"
            /// <summary>
            /// This is the class documentation.
            /// </summary>
            class HandlerTest
            {
                /// <summary>
                /// This is the method's documentation.
                /// </summary>
                /// <param name=""value"">This is the argument documentation.</param>
                /// <returns>This is the return documentation.</returns>
                public int? DoWork(bool value)
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

            var definition = RequestHandlerDefinition.TryParse(methodSymbol);

            Assert.IsNotNull(definition);

            Assert.IsNotNull(definition.RequestType);
            Assert.AreEqual("This is the method's documentation.", definition.RequestType.Documentation);
            Assert.AreEqual("This is the argument documentation.", definition.RequestType.Members.First().Documentation);

            Assert.IsNotNull(definition.ResponseType);
            Assert.AreEqual("The response for {DoWorkRequest}.", definition.ResponseType.Documentation);
            Assert.AreEqual("This is the return documentation.", definition.ResponseType.Members.First().Documentation);
        }

        [TestMethod]
        public void TryParse_PartialDocumentation_ValidMethod()
        {
            var code = @"
            /// <summary>
            /// This is the class documentation.
            /// </summary>
            class HandlerTest
            {
                /// <summary>
                /// This is the method's documentation.
                /// </summary>
                public int? DoWork(bool value)
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

            var definition = RequestHandlerDefinition.TryParse(methodSymbol);

            Assert.IsNotNull(definition);

            Assert.IsNotNull(definition.RequestType);
            Assert.AreEqual("This is the method's documentation.", definition.RequestType.Documentation);
            Assert.AreEqual(string.Empty, definition.RequestType.Members.First().Documentation);

            Assert.IsNotNull(definition.ResponseType);
            Assert.AreEqual("The response for {DoWorkRequest}.", definition.ResponseType.Documentation);
            Assert.AreEqual(string.Empty, definition.ResponseType.Members.First().Documentation);
        }

        [TestMethod]
        public void TryParse_NoDocumentation_ValidMethod()
        {
            var code = @"
            class HandlerTest
            {
                public int? DoWork(bool value)
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

            var definition = RequestHandlerDefinition.TryParse(methodSymbol);

            Assert.IsNotNull(definition);

            Assert.IsNotNull(definition.RequestType);
            Assert.AreEqual(string.Empty, definition.RequestType.Documentation);
            Assert.AreEqual(string.Empty, definition.RequestType.Members.First().Documentation);

            Assert.IsNotNull(definition.ResponseType);
            Assert.AreEqual("The response for {DoWorkRequest}.", definition.ResponseType.Documentation);
            Assert.AreEqual(string.Empty, definition.ResponseType.Members.First().Documentation);
        }
    }
}
