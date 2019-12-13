using System;

namespace Identity.Dapper.Postgres.Models
{
    internal class RoleClaim
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
