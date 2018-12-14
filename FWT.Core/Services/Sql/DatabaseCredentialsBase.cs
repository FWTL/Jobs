namespace FWTL.Services.Sql
{
    using FWTL.Core.Sql;

    public abstract class DatabaseCredentialsBase : IDatabaseCredentials
    {
        protected DatabaseCredentialsBase()
        {
        }

        public string ConnectionString { get; private set; }

        public void BuildConnectionString(string url, int port, string catalog, string userName, string password)
        {
            ConnectionString = $"Server=tcp:{url},{port};Initial Catalog={catalog};Persist Security Info=False;User ID={userName};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
        }

        public void BuildLocalConnectionString(string url, string catalog)
        {
            ConnectionString = $"Data Source={url};Initial Catalog={catalog};Persist Security Info=False;Integrated Security=True;";
        }
    }
}
