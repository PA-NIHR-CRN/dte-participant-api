using System;
using System.Net;
using Dte.Api.Acceptance.Test.Helpers.Clients;
using Dte.Api.Acceptance.Test.Helpers.Extensions;
using Dte.Common.Authentication;
using Dte.Common.Http;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ParticipantApi.Acceptance.Tests.Clients;

namespace ParticipantApi.Acceptance.Tests
{
    public abstract class AcceptanceTestBase
    {
        private ApiClient _apiClient;
        
        // Override this method to provide a custom claims
        protected virtual void CreateApiWebApplicationFactory()
        {
            TestApi = new ApiWebApplicationFactory();
        }
        
        [SetUp]
        public void Setup()
        {
            CreateApiWebApplicationFactory();
            Scope = TestApi.Services.CreateScope();

            AuthenticationSettings = Scope.ServiceProvider.GetService<AuthenticationSettings>();
            
            var httpClient = TestApi.CreateClient();
            _apiClient = ApiClientFactory.For(httpClient, "test");
            BaseAddress = httpClient.BaseAddress;
            
            ParticipantApiClient = new ParticipantApiClient(_apiClient);
        }
        
        protected ApiWebApplicationFactory TestApi;
        protected IServiceScope Scope { get; private set; }
        protected Uri BaseAddress { get; private set; }
        protected ParticipantApiClient ParticipantApiClient { get; private set; }
        protected AuthenticationSettings AuthenticationSettings { get; private set; }

        protected static void AssertResponseStatusCode(IStubApiClient client, HttpStatusCode statusCode)
        {
            client.LastResponse().ShouldHaveStatusCode(statusCode);
        }

        protected static void AssertResponseContentType(IStubApiClient client, string contentType)
        {
            client.LastResponse().ShouldHaveContentType(contentType);
        }

        [TearDown]
        public void Dispose()
        {
            Scope.Dispose();
            TestApi?.Dispose();
            _apiClient?.Dispose();
        }
    }
}