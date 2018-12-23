namespace FWTL.Database.Configuration
{
    using FWTL.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.Property(x => x.UserId).IsRequired();
            builder.HasIndex(x => x.UserId);
        }
    }
}
