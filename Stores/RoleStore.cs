using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Identity.Dapper.Postgres.Repositories;
using Identity.Dapper.Postgres.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Dapper.Postgres.Stores
{
    /// <summary>
    /// Role data access layer
    /// </summary>
    public class RoleStore : IQueryableRoleStore<ApplicationRole>, IRoleClaimStore<ApplicationRole>, IRoleStore<ApplicationRole>
    {
        private readonly RolesRepository _rolesRepository;
        private readonly RoleClaimsRepository _roleClaimsRepository;

        public RoleStore(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _rolesRepository = new RolesRepository(databaseConnectionFactory);
            _roleClaimsRepository = new RoleClaimsRepository(databaseConnectionFactory);
        }

        #region IQueryableRoleStore Implementation

        public IQueryable<ApplicationRole> Roles => Task.Run(() => _rolesRepository.GetAllRolesAsync()).Result.AsQueryable();
        #endregion

        #region IRoleStore<ApplicationRole> Implementation

        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return _rolesRepository.CreateAsync(role);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return _rolesRepository.UpdateAsync(role);
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return _rolesRepository.DeleteAsync(role);
        }


        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return Task.FromResult(role.Id.ToString());
        }


        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return Task.FromResult(role.Name);
        }


        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            roleName.ThrowIfNull(nameof(roleName));
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            normalizedName.ThrowIfNull(nameof(normalizedName));
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            roleId.ThrowIfNull(nameof(roleId));
            var isValidGuid = Guid.TryParse(roleId, out var roleGuid);

            if (!isValidGuid)
            {
                throw new ArgumentException("Parameter roleId is not a valid Guid.", nameof(roleId));
            }

            return _rolesRepository.FindByIdAsync(roleGuid);
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            normalizedRoleName.ThrowIfNull(nameof(normalizedRoleName));
            return _rolesRepository.FindByNameAsync(normalizedRoleName);
        }


        public void Dispose() { /* Nothing to dispose. */ }
        #endregion

        #region IRoleClaimStore<ApplicationRole> Implementation

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            role.Claims = role.Claims ?? (await _roleClaimsRepository.GetClaimsAsync(role.Id)).ToList();
            return role.Claims;
        }

        public async Task AddClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            claim.ThrowIfNull(nameof(claim));
            role.Claims = role.Claims ?? (await _roleClaimsRepository.GetClaimsAsync(role.Id)).ToList();
            var foundClaim = role.Claims.FirstOrDefault(x => x.Type == claim.Type);

            if (foundClaim != null)
            {
                role.Claims.Remove(foundClaim);
                role.Claims.Add(claim);
            }
            else
            {
                role.Claims.Add(claim);
            }
        }

        public async Task RemoveClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            claim.ThrowIfNull(nameof(claim));
            role.Claims = role.Claims ?? (await _roleClaimsRepository.GetClaimsAsync(role.Id)).ToList();
            role.Claims.Remove(claim);
        }
        #endregion
    }
}
