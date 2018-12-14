using System;
using FWTL.Core.Services.Unique;

namespace FWTL.Infrastructure.Services.Unique
{
    public class RandomService : IRandomService
    {
        public Random Random { get; } = new Random();
    }
}