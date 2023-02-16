using System;
using System.Linq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Application.Contracts;
using Application.Settings;
using Dte.Common;
using Dte.Common.Contracts;
using Dte.Common.Http;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ParticipantApi.DependencyRegistrations
{
    public static class InfrastructureRegistration
    {
        private static readonly string[] ProdEnvironmentNames = { "production", "prod", "live"};
        
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string environmentName)
        {
            // Infrastructure dependencies
            services.AddScoped<IStudyRepository, StudyDynamoDbRepository>();
            services.AddScoped<IParticipantRegistrationRepository, ParticipantRegistrationDynamoDbRepository>();
            services.AddScoped<IParticipantRepository, ParticipantDynamoDbRepository>();
            services.AddSingleton<IClock, Clock>();
            services.AddSingleton<IHeaderService, HeaderService>();
            
            // AWS
            var awsSettings = configuration.GetSection(AwsSettings.SectionName).Get<AwsSettings>();
            var amazonDynamoDbConfig = new AmazonDynamoDBConfig();
            if (!string.IsNullOrWhiteSpace(awsSettings.ServiceUrl))
            {
                amazonDynamoDbConfig.ServiceURL = awsSettings.ServiceUrl;
            }

            services.AddScoped<IAmazonDynamoDB>(_ => new AmazonDynamoDBClient(amazonDynamoDbConfig));
            services.AddScoped<IDynamoDBContext>(_ => new DynamoDBContext(new AmazonDynamoDBClient(amazonDynamoDbConfig)));
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());

            // If not Prod, then enable stubs
            if(!ProdEnvironmentNames.Any(x => string.Equals(x, environmentName, StringComparison.OrdinalIgnoreCase)))
            {
                // Enable local stubs
            }
            
            return services;
        }
    }
}