namespace FWTL.Database
{
    using FWTL.Database.Configuration;
    using Microsoft.EntityFrameworkCore;

    public class TelegramDatabaseContext : DbContext
    {
        private readonly JobsDatabaseCredentials _credentials;

        public TelegramDatabaseContext(JobsDatabaseCredentials credentials)
        {
            _credentials = credentials;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_credentials.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new JobConfiguration());
        }
    }
}
