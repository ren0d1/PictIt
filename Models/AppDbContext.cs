namespace PictIt.Models
{
    using System;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
