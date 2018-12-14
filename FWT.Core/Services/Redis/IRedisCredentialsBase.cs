namespace FWTL.Core.Services.Redis
{
    public interface IRedisCredentialsBase
    {
        string ConnectionString { get; }
    }
}
