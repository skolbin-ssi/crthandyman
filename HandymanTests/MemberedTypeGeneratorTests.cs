using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handyman.Generators;
using Handyman.Tests.TestHelpers;
using Handyman.Types;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Handyman.Tests
{
    [TestClass]
    public class MemberedTypeGeneratorTests
    {
        [TestMethod]
        public void GenerateSyntax_ValidCode()
        {
            var symbol = MockRepository.GenerateMock<ITypeSymbol>();
            symbol.Stub(s => s.Name).Return("int");

            string code = new MemberedTypeGenerator().GenerateSyntax(new RequestType("Sample", new[] { new Member("id", symbol) }));

            // If it doesn't throw, we are good
            RoslynHelper.Compile(code);
        }
    }
}
