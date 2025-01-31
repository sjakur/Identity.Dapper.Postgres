﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Identity.Dapper.Postgres.Models;

namespace Identity.Dapper.Postgres.Repositories
{
    internal class RoleClaimsRepository
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public RoleClaimsRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        public async Task<IList<Claim>> GetClaimsAsync(Guid roleId)
        {
            const string command = "SELECT * " +
                                   "FROM identity_role_claims " +
                                   "WHERE role_id = @RoleId;";

            IEnumerable<RoleClaim> roleClaims = new List<RoleClaim>();

            using (var sqlConnection = await _databaseConnectionFactory.CreateConnectionAsync())
            {
                return (
                    await sqlConnection.QueryAsync<RoleClaim>(command, new { RoleId = roleId })
                )
                .Select(x => new Claim(x.ClaimType, x.ClaimValue))
                .ToList();
            }
        }
    }
}
