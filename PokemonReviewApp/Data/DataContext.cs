using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace PokemonReviewApp.Data
{
    public class DataContext : DbContext
    {
        private readonly IHttpContextAccessor _http;

        public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor http)
            : base(options)
        {
            _http = http;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<PokeFood> PokeFoods { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PokeProperty> PokeProperties { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<PokemonLog> PokemonLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ----------------------------
            // GLOBAL QUERY FILTER (ONLY ONCE!)
            // ----------------------------
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditEntityBase).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "x");
                    var prop = Expression.Property(parameter, nameof(AuditEntityBase.IsDeleted));
                    var compare = Expression.Equal(prop, Expression.Constant(false));
                    var lambda = Expression.Lambda(compare, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            // Seeder
            DbSeeder.Seed(modelBuilder);

            // ----------------------------
            // PokemonCategory
            // ----------------------------
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(pc => new { pc.PokemonId, pc.CategoryId });

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);

            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(c => c.CategoryId);

            // ----------------------------
            // PokemonOwner
            // ----------------------------
            modelBuilder.Entity<PokemonOwner>()
                .HasKey(po => new { po.PokemonId, po.OwnerId });

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);

            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Owner)
                .WithMany(po => po.PokemonOwners)
                .HasForeignKey(c => c.OwnerId);

            // ----------------------------
            // PokeFood
            // ----------------------------
            modelBuilder.Entity<PokeFood>()
                .HasKey(pf => new { pf.PokemonId, pf.FoodId });

            modelBuilder.Entity<PokeFood>()
                .HasOne(p => p.Pokemon)
                .WithMany(pf => pf.PokeFoods)
                .HasForeignKey(p => p.PokemonId);

            modelBuilder.Entity<PokeFood>()
                .HasOne(f => f.Food)
                .WithMany(pf => pf.PokeFoods)
                .HasForeignKey(f => f.FoodId);

            // ----------------------------
            // PokeProperty
            // ----------------------------
            modelBuilder.Entity<PokeProperty>()
                .HasKey(pp => new { pp.PokemonId, pp.PropertyId });

            modelBuilder.Entity<PokeProperty>()
                .HasOne(p => p.Pokemon)
                .WithMany(pp => pp.PokeProperties)
                .HasForeignKey(p => p.PokemonId);

            modelBuilder.Entity<PokeProperty>()
                .HasOne(pr => pr.Property)
                .WithMany(pp => pp.PokeProperties)
                .HasForeignKey(pr => pr.PropertyId);

            // ----------------------------
            // User → Role (1 - N)
            // ----------------------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----------------------------
            // RolePermission (N - N)
            // ----------------------------
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
        {
            int? currentUserId = null;
            var idClaim = _http?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim != null && int.TryParse(idClaim.Value, out var parsed)) currentUserId = parsed;

            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditEntityBase &&
                e.Entity.GetType().Name.EndsWith("Log") == false &&
                            (e.State == EntityState.Added ||
                             e.State == EntityState.Modified ||
                             e.State == EntityState.Deleted));

            var now = DateTime.Now;

            foreach (var entry in entries)
            {
                var entity = (AuditEntityBase)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDateTime = now;
                    if (currentUserId.HasValue) entity.CreatedUserId = currentUserId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedDateTime = now;
                    if (currentUserId.HasValue) entity.UpdatedUserId = currentUserId;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entity.IsDeleted = true;
                    entity.DeletedDateTime = now;
                    if (currentUserId.HasValue) entity.DeletedUserId = currentUserId;
                }
            }
        }
    }
}
