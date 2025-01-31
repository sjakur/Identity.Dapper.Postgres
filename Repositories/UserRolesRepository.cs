﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Identity.Dapper.Postgres.Models;

namespace Identity.Dapper.Postgres.Repositories
{
    internal class UserRolesRepository
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UserRolesRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        public async Task<IEnumerable<UserRole>> GetRolesAsync(ApplicationUser user)
        {
            const string command = "SELECT r.id AS role_id, r.name AS role_name " +
                                   "FROM identity_roles AS r " +
                                   "INNER JOIN identity_user_roles AS ur ON ur.role_id = r.id " +
                                   "WHERE ur.user_id = @UserId;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                return await sqlConnection.QueryAsync<UserRole>(command, new
                {
                    UserId = user.Id
                });
            }
        }
    }
}
