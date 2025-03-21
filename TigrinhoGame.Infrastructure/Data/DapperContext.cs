using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace TigrinhoGame.Infrastructure.Data
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IOptions<DatabaseConfig> config)
        {
            _connectionString = config.Value.ConnectionString;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
} 