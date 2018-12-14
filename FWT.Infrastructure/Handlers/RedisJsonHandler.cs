using System;
using System.Threading.Tasks;
using FWTL.Core.CQRS;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace FWTL.Infrastructure.Handlers
{
    public class RedisJsonHandler<TQuery, TResult> : IReadCacheHandler<TQuery, TResult>, IWriteCacheHandler<TQuery, TResult> where TQuery : IQuery where TResult : class
    {
        private readonly IDatabase _cache;

        public RedisJsonHandler(IDatabase cache)
        {
            _cache = cache;
        }

        public Func<TQuery, string> KeyFn { get; set; }

        public virtual async Task<TResult> ReadAsync(TQuery query)
        {
            if (KeyFn == null)
            {
                throw new ArgumentException("KeyFn not defined");
            }

            string key = KeyFn(query);
            if (string.IsNullOrWhiteSpace(key))
            {
                return default(TResult);
            }

            var cache = await _cache.StringGetAsync(key).ConfigureAwait(false);

            if (!cache.HasValue)
            {
                return default(TResult);
            }

            return JsonConvert.DeserializeObject<TResult>(cache.ToString());
        }

        public virtual TimeSpan? Ttl(TQuery query)
        {
            return null;
        }

        public virtual async Task WriteAsync(TQuery query, TResult result)
        {
            if (KeyFn == null)
            {
                throw new Exception("KeyFn not defined");
            }

            string key = KeyFn(query);
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            var cacheExpiration = Ttl(query);
            await _cache.StringSetAsync(key, JsonConvert.SerializeObject(result), cacheExpiration).ConfigureAwait(false);
        }
    }
}
