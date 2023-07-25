using Microsoft.Extensions.DependencyInjection;
using Personio.Api.Configuration;
using System;

namespace Personio.Api.DependencyInjection
{
    public class PersonioClientOptionsBuilder
    {
        internal PersonioClientOptions Options = new PersonioClientOptions();

        public PersonioClientOptionsBuilder UseClientSecret(string clientSecret)
        {
            Options.ClientSecret = clientSecret;
            return this;
        }

        public PersonioClientOptionsBuilder UseClientId(string clientId)
        {
            Options.ClientId = clientId;
            return this;
        }

        public PersonioClientOptionsBuilder UseAppId(string appId)
        {
            Options.AppId = appId;
            return this;
        }

        public PersonioClientOptionsBuilder UsePartnerId(string partnerId)
        {
            Options.PartnerId = partnerId;
            return this;
        }
    }

    public static class PersonioClientExtensions
    {
        public static void AddPersonioClient(this IServiceCollection services, Action<PersonioClientOptionsBuilder> builderAction)
        {
            var builder = new PersonioClientOptionsBuilder();
            if (builderAction != null)
            {
                builderAction(builder);
            }

            services.AddSingleton(builder.Options);
            services.AddSingleton<PersonioClient>();
        }
    }
}