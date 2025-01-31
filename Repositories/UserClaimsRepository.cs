﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Identity.Dapper.Postgres.Models;

namespace Identity.Dapper.Postgres.Repositories
{
    internal class UserClaimsRepository
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public UserClaimsRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            const string command = "SELECT * " +
                                   "FROM identity_user_claims " +
                                   "WHERE user_id = @UserId;";

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                return (
                    await sqlConnection.QueryAsync<UserClaim>(command, new { UserId = user.Id })
                )
                .Select(e => new Claim(e.ClaimType, e.ClaimValue))
                .ToList(); ;
            }
        }
    }
}
