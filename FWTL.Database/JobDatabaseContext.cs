namespace FWTL.Database
{
    using FWTL.Database.Configuration;
    using Microsoft.EntityFrameworkCore;

    public class JobDatabaseContext : DbContext
    {
        private readonly JobDatabaseCredentials _credentials;

        public JobDatabaseContext(JobDatabaseCredentials credentials)
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
