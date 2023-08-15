using Microsoft.Extensions.DependencyInjection;
using Personio.Api;
using Personio.Api.DependencyInjection;
using System;
using Xunit;

namespace Personio.Tests
{
    public class BaseUnitTest
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly PersonioClient PersonioClient;
        public BaseUnitTest()
        {
            var services = new ServiceCollection();
            services.AddPersonioClient(options => options.UseClientId("MzgxZWRhYzJhMTljMWE3YjAzN2U4NjJm").UseClientSecret("ZDdhYmQ0Y2FjZTlmOTllZTUzYmU2NDJmYzM0N2M5MTkxMDcy"));
            ServiceProvider = services.BuildServiceProvider();
            PersonioClient = ServiceProvider.GetRequiredService<PersonioClient>();
        }

        [Fact]
        public void DITest()
        {
            Assert.NotNull(ServiceProvider);
            Assert.NotNull(PersonioClient);
        }
    }
}