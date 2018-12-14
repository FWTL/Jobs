using System;
using FWTL.Core.Services.Unique;

namespace FWTL.Infrastructure.Unique
{
    public class GuidService : IGuidService
    {
        public Guid New()
        {
            return Guid.NewGuid();
        }
    }
}
