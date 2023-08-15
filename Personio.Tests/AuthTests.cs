using Personio.Api;
using Xunit;

namespace Personio.Tests
{
    public class AuthTests : BaseUnitTest
    {
        [Fact]
        public void AuthTest()
        {
            var token = PersonioClient.AuthAsync().Result;
            Assert.NotNull(token);
        }
    }
}