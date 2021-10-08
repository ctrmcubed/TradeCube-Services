namespace Shared.Redis
{
    public class ScafellRedisHost
    {
        public string Host { get; }

        public int Port { get; }

        public ScafellRedisHost(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}