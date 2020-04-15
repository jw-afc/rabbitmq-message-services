namespace MessageConsumer.Options
{
    public class RabbitOptions
    {
        public string HostName { get; set; }
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public string RouteKey { get; set; }
    }
}
