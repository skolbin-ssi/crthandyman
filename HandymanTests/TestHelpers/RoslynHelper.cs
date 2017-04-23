using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace HandymanTests.TestHelpers
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

        public static Compilation ParseWithBaseTypes(string code)
        {
            var trees = BaseTypesSyntaxTree.Concat(new[] { ParseText(code) }).ToArray();

            return CSharpCompilation.Create("test", trees);
        }

        private static SyntaxTree[] ParseBaseTypes()
        {
            string cd = Directory.GetCurrentDirectory();
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
