using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParticipantApi.Extensions;

namespace ParticipantApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .AddAwsSecrets()
                .ConfigureLogging(logger => {
                    var options = new LambdaLoggerOptions
                    {
                        IncludeException = true,
                    };

                    logger.AddLambdaLogger(options);
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureAppConfiguration((hostContext, configurationBuilder) => configurationBuilder.AddUserSecrets<Program>())
            ;
    }
}