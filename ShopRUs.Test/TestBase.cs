using NUnit.Framework;
using System.Threading.Tasks;

namespace ShopsRUs.Test
{
    using static Testing;
    public class TestBase
    {
        [SetUp]
        public async Task SetupAsync()
        {
            await ResetDbState();
        }
        [TearDown]
        public async Task TearDownAsync()
        {
            await ResetDbState();
        }
    }
}