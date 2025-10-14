using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<User, AppRole, Guid, 
                                 IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, 
                                 IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tyre> Tyres { get; set; }
        public DbSet<User> Workers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<BusinessUnitLeader> BusinessUnitLeaders { get; set; }
        public DbSet<QualitySupervisor> QualitySupervisors { get; set; }
        public DbSet<ProductionOperator> ProductionOperators { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
            .HasMany(user => user.UserRoles)
            .WithOne(userRole => userRole.User)
            .HasForeignKey(userRole => userRole.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

            builder.Entity<AppRole>()
            .HasMany(role => role.UserRoles)
            .WithOne(userRole => userRole.Role)
            .HasForeignKey(userRole => userRole.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
            
            builder.Entity<Production>()
                .HasOne(p => p.Tyre)
                .WithMany(t => t.Productions);

            builder.Entity<Production>()
                .HasOne(p => p.Operator)
                .WithMany(o => o.Productions);

            builder.Entity<Production>()
                .HasOne(p => p.Machine)
                .WithMany(m => m.Productions);
            
            builder.Entity<Sale>()
                .HasOne(s => s.Tyre)
                .WithMany(t => t.Sales);
            
            builder.Entity<Sale>()
                .HasOne(s => s.Client)
                .WithMany(c => c.Sales);

            builder.Entity<Sale>()
                .HasOne(s => s.Production)
                .WithMany(c => c.Sales);
            
            builder.Entity<Report>()
                .HasOne(r => r.BusinessUnitLeader)
                .WithMany(b => b.Reports);
            
            // builder.Entity<Production>()
            //     .HasOne(p => p.Supervisor)
            //     .WithMany(q => q.Productions);
        }
    }
}