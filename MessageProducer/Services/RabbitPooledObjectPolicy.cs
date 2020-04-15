using MessageProducer.Options;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MessageProducer.Services
{
    public class RabbitPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitOptions _options;
        private readonly IConnection _connection;

        public RabbitPooledObjectPolicy(IOptions<RabbitOptions> options)
        {
            _options = options.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                VirtualHost = _options.VHost,
            };

            return factory.CreateConnection();
        }

        public IModel Create() => _connection.CreateModel();

        public bool Return(IModel obj)
        {
            if (obj.IsOpen) return true;
            
            obj?.Dispose();
            return false;
        }
    }
}
