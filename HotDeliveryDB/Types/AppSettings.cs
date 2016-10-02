namespace HotDeliveryDB
{
    public class AppSettings
    {
        public int DeliveriesCountMin { get; set; }
        public int DeliveriesCountMax { get; set; }
        public int TaskIntervalMin { get; set; }
        public int TaskIntervalMax { get; set; }
        public int ExpirationTime { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
    }

    public class ConnectionStrings
    {
        public string DBFormat { get; set; }
        public string Path { get; set; }
    }
}
