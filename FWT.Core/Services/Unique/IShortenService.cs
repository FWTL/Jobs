using HashidsNet;

namespace FWTL.Core.Services.Hash
{
    public interface IShortenService
    {
        Hashids Hash<TModel>();
    }
}
