using System.Data.SqlClient;

namespace FakeBusinessLogic
{
    public class DapperConnectionManager
    {
        private readonly AppConfiguration _configuration;

        public DapperConnectionManager(AppConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSqlConnectionString()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                ApplicationName = "POC",
                DataSource = $"{_configuration.DbHostName},{_configuration.DbPort}",
                UserID = _configuration.DbUserName,
                Password = _configuration.DbPassword,
                InitialCatalog = _configuration.DbInitialCatalog,
                MultipleActiveResultSets = true
            };

            return connectionStringBuilder.ToString();
        }
    }
}
