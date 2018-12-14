namespace FWTL.Core.Services.Redis
{
    public abstract class RedisCredentialsBase : IRedisCredentialsBase
    {
        public string ConnectionString { get; private set; }

        public void BuildConnectionString(string name, string password, int port, bool isSsl, bool allowAdmin)
        {
            ConnectionString = $"{name}.redis.cache.windows.net:{port},password={password},ssl={isSsl},abortConnect=False,allowAdmin={allowAdmin}";
        }
    }
}
