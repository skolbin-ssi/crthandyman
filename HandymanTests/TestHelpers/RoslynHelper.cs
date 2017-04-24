using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Handyman.Tests.TestHelpers
{
    public class RoslynHelper
    {
        private static SyntaxTree[] BaseTypesSyntaxTree = ParseBaseTypes();
        private static string RuntimePath = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.1\{0}.dll";

        private static readonly IEnumerable<MetadataReference> DefaultReferences =
            new[]
            {
                MetadataReference.CreateFromFile(string.Format(RuntimePath, "mscorlib")),
                MetadataReference.CreateFromFile(string.Format(RuntimePath, "System")),
                MetadataReference.CreateFromFile(string.Format(RuntimePath, "System.Core"))
            };

        private static readonly CSharpCompilationOptions DefaultCompilationOptions =
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                    .WithOverflowChecks(true).WithOptimizationLevel(OptimizationLevel.Debug);

        public static Compilation Compile(string code)
        {
            SyntaxTree s;
            return Compile(code, out s);
        }

        public static Compilation Compile(string code, out SyntaxTree codeSyntaxTree)
        {
            codeSyntaxTree = ParseText(code);
            var trees = BaseTypesSyntaxTree.Concat(new[] { codeSyntaxTree }).ToArray();
            var compilation = CSharpCompilation.Create("test", trees, DefaultReferences, DefaultCompilationOptions);

            var errors = compilation.GetDiagnostics();

            if (errors.Any())
            {
                throw new ArgumentException($"Compilation errors: { string.Join("\n", errors.Select(e => e.GetMessage())) }");
            }

            return compilation;
        }

        private static SyntaxTree[] ParseBaseTypes()
        {
            string cd = Path.Combine(Directory.GetCurrentDirectory(), "BaseTypes");
            List<SyntaxTree> trees = new List<SyntaxTree>();

            foreach (string path in Directory.GetFiles(cd, "*.cs"))
            {
                trees.Add(ParseFile(path));
            }

            return trees.ToArray();
        }

        private static SyntaxTree ParseFile(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return SyntaxFactory.ParseSyntaxTree(SourceText.From(stream, Encoding.UTF8));
            }
        }

        private static SyntaxTree ParseText(string text)
        {
            return SyntaxFactory.ParseSyntaxTree(SourceText.From(text));
        }
    }
}
