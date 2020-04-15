using System.Threading;
using System.Threading.Tasks;
using MessageProducer.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessageProducer
{
    /// <summary>
    /// https://www.c-sharpcorner.com/article/publishing-rabbitmq-message-in-asp-net-core/
    /// </summary>
    internal class MessageProducerService : IHostedService
    {
        private ILogger<MessageProducerService> Logger { get; }
        private IRabbitManager RabbitManager { get; }
        private CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        private TaskCompletionSource<bool> TaskCompletionSource { get; } = new TaskCompletionSource<bool>();

        public MessageProducerService(ILogger<MessageProducerService> logger, IRabbitManager rabbitManager)
        {
            Logger = logger;
            RabbitManager = rabbitManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Start our application code.
            Task.Run(() => PublishMessages(CancellationTokenSource.Token));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            CancellationTokenSource.Cancel();
            // Defer completion promise, until our application has reported it is done.
            return TaskCompletionSource.Task;
        }

        public async Task PublishMessages(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var num = new System.Random().Next(9000);

                var content = new { field1 = $"Hello-{num}", field2 = $"rabbit-{num}" };

                Logger.LogInformation($"producer published {content}");

                // publish message  
                RabbitManager.Publish(content, "demo.exchange.topic.dotnetcore", "topic", "*.queue.durable.dotnetcore.#");

                await Task.Delay(100);
            }
            Logger.LogInformation("Stopping");
            TaskCompletionSource.SetResult(true);
        }
    }
}
