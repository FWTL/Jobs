namespace FWTL.Core.Entities.Maps
{
    public static class JobMap
    {
        public static readonly string Id = nameof(Job.Id);
        public static readonly string PeerId = nameof(Job.PeerId);
        public static readonly string PeerType = nameof(Job.PeerType);
        public static readonly string UserId = nameof(Job.UserId);
        public static readonly string State = nameof(Job.State);
        public static readonly string MaxId = nameof(Job.MaxId);

        public static readonly string Table = nameof(Job);
    }
}