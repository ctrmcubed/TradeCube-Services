using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace Shared.Services.Redis
{
    public class RedisService : IRedisService
    {
        private readonly RedisConfiguration redisConfiguration;
        private readonly RedisCacheClient redisCacheClient;
        
        private static readonly ILogger Logger = Log.ForContext(typeof(RedisService));
        
        public RedisService(RedisConfiguration redisConfiguration)
        {
            this.redisConfiguration = redisConfiguration;

            var merged = MergeConfiguration(redisConfiguration, RedisHosts(), RedisPassword());

            redisCacheClient = new RedisCacheClient(new RedisCacheConnectionPoolManager(merged), new SystemTextJsonSerializer(), merged);
        }

        public ConnectionMultiplexer ConnectionMultiplexer()
        {
            var environmentVariableHosts = RedisHosts();
            var merged = MergeConfiguration(redisConfiguration, environmentVariableHosts, RedisPassword());
            var redis = StackExchange.Redis.ConnectionMultiplexer.Connect(merged.ConfigurationOptions.ToString());

            Logger.Information("Redis config: {RedisConfiguration}", redis.Configuration);

            return redis;
        }

        private static IEnumerable<ScafellRedisHost> RedisHosts(string hosts)
        {
            IEnumerable<ScafellRedisHost> Hosts()
            {
                return hosts
                    .Split(';')
                    .Select(s => s.Split(','))
                    .Select(s => new ScafellRedisHost(s[0], int.Parse(s[1])));
            }

            return string.IsNullOrWhiteSpace(hosts)
                ? new List<ScafellRedisHost>()
                : Hosts();
        }

        public async Task<T> Get<T>(params string[] keys)
        {
            return await redisCacheClient.GetDbFromConfiguration().GetAsync<T>(CreateKey(keys));
        }

        public async Task Set<T>(T item, params string[] keys)
        {
            await redisCacheClient.GetDbFromConfiguration().AddAsync(CreateKey(keys), item);
        }

        public async Task Set<T>(T item, int expiresInHours, params string[] keys)
        {
            await redisCacheClient.GetDbFromConfiguration().AddAsync(CreateKey(keys), item, TimeSpan.FromHours(expiresInHours));
        }

        public async Task Clear(params string[] keys)
        {
            await redisCacheClient.GetDbFromConfiguration().RemoveAsync(CreateKey(keys));
        }

        private static IEnumerable<ScafellRedisHost> RedisHosts() =>
            RedisHosts(Environment.GetEnvironmentVariable("TRADECUBE_SERVICES_REDIS_HOSTS"));

        private static string RedisPassword() =>
            Environment.GetEnvironmentVariable("TRADECUBE_SERVICES_REDIS_PASSWORD");

        private static RedisConfiguration MergeConfiguration(RedisConfiguration redisConfiguration, IEnumerable<ScafellRedisHost> environmentVariableHosts, string environmentPassword)
        {
            return new RedisConfiguration
            {
                AbortOnConnectFail = redisConfiguration.AbortOnConnectFail,
                KeyPrefix = redisConfiguration.KeyPrefix,
                Hosts = redisConfiguration.Hosts == null
                    ? Map(environmentVariableHosts).ToArray()
                    : Map(environmentVariableHosts).Concat(redisConfiguration.Hosts).ToArray(),
                AllowAdmin = redisConfiguration.AllowAdmin,
                ConnectTimeout = redisConfiguration.ConnectTimeout,
                Database = redisConfiguration.Database,
                ServerEnumerationStrategy = redisConfiguration.ServerEnumerationStrategy,
                Password = environmentPassword
            };
        }

        private static string CreateKey(params string[] keys)
        {
            return string.Join(":", keys);
        }

        private static IEnumerable<RedisHost> Map(IEnumerable<ScafellRedisHost> hosts)
        {
            return hosts.Select(h => new RedisHost { Host = h.Host, Port = h.Port });
        }
    }
}
