using System;
using HandymanTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HandymanTests
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

            var compilation = RoslynHelper.ParseWithBaseTypes(code);
        }
    }
}
