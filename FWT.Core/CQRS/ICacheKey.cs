using System;

namespace FWTL.Core.CQRS
{
    public interface ICacheKey<TQuery>
    {
        Func<TQuery, string> KeyFn { get; set; }
    }
}
