using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Identity.Dapper.Postgres.Models
{
    public class ApplicationRole
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        internal List<Claim> Claims { get; set; }
    }
}
