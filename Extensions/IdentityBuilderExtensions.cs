
using Identity.Dapper.Postgres.Stores;
using Identity.Dapper.Postgres.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Dapper.Postgres
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddDapperStores(this IdentityBuilder builder, string connectionString)
        {
            AddStores(builder.Services, connectionString);
            return builder;
        }

        private static void AddStores(IServiceCollection services, string connectionString)
        {
            services.AddScoped<IUserStore<ApplicationUser>, UserStore>();
            services.AddScoped<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddScoped<IDatabaseConnectionFactory>(provider => new PostgresConnectionFactory(connectionString));
        }
    }
}
