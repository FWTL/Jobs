using System;
using NodaTime;

namespace FWTL.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime UtcNow(this IClock clock)
        {
            return clock.GetCurrentInstant().InUtc().ToDateTimeUtc();
        }
    }
}
