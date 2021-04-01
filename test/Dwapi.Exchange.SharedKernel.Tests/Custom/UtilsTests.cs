using Dwapi.Exchange.SharedKernel.Custom;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Exchange.SharedKernel.Tests.Custom
{
    [TestFixture]
    public class UtilsTests
    {
        [TestCase(1,10,1,10)]
        [TestCase(2,10,11,20)]
        [TestCase(3,10,21,30)]
        [TestCase(4,10,31,40)]
        [TestCase(2,6,7,12)]
        public void should_creat_extract_block(long pg,long pgSize,long first,long last)
        {
            var block = Utils.CreateBlock(pg, pgSize);
            Assert.True(first<=block.First && first<=block.Last);
            Assert.True(last<=block.Last && last>=block.First);
            Log.Debug($"pg:{pg} size:{pgSize}, {block}");
        }
    }
}
