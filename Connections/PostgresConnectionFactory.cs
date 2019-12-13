using System;
using System.Data;
using Npgsql;
using System.Threading.Tasks;
using Dapper;

namespace Identity.Dapper.Postgres
{
    public class PostgresConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public PostgresConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var sqlConnection = new NpgsqlConnection(_connectionString);
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await sqlConnection.OpenAsync();
            return sqlConnection;
        }
    }
}
