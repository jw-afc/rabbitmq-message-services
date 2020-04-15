using System;
using System.Threading.Tasks;
using MessageConsumer.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MessageConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();


            return Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    var rabbitConfig = configuration.GetSection("Rabbit");
                    services.Configure<RabbitOptions>(rabbitConfig);

                    services.AddHostedService<MessageConsumerService>();
                });
        }
    }
}
