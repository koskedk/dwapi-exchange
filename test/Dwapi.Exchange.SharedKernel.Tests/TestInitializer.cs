using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.SharedKernel.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
       
        [OneTimeSetUp]
        public void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
