using Dwapi.Exchange.SharedKernel.Custom;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.SharedKernel.Tests.Custom
{
   [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void should_ensure_string_Ends()
        {
            string a = "A";
            Assert.AreEqual("A-", a.HasToEndWith("-"));
        }

        [Test]
        public void should_change_To_OsPath()
        {
            var dir = TestContext.CurrentContext.WorkDirectory.ToOsStyle();
            Assert.False(string.IsNullOrWhiteSpace(dir));
            Log.Debug(dir);
        }
    }
}