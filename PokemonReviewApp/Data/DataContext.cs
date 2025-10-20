using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Models;
namespace PokemonReviewApp.Data
{

    //DATA LAYER, thanks to Datacontext we can access database and explanation of tables
    //This folder is brain of the project
    //its a bridge with models and SQL Server

    public class DataContext : DbContext //inherit from Dbcontext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        //let me tell you a story about RELATIONSHIPSS of ENTITY
        // OnModelCreating : special code of DBCONTEXT. it says that, when we create database we define some rules manually.
        //relationship exist with this part. it explains relationships to ENTITY FRAMEWORK
        {
            modelBuilder.Entity<PokemonCategory>() //Creating join table for pokemon and category.
                .HasKey(pc => new { pc.PokemonId, pc.CategoryId }); //COMPOSITE KEY -> define unique register with pokemonID and CategoryID
            //Thanks to HasKEY you cant add a same pokemon-category duo.
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon) // p refers to every pokemon in PokemonCategory table. 
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>()
              .HasOne(p => p.Category)
              .WithMany(pc => pc.PokemonCategories)
              .HasForeignKey(c => c.CategoryId);
            modelBuilder.Entity<PokemonOwner>()
               .HasKey(po => new { po.PokemonId, po.OwnerId });
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
              .HasOne(p => p.Owner)
              .WithMany(pc => pc.PokemonOwners)
              .HasForeignKey(c => c.OwnerId);

            //HasOne kullanırken aslında diyorum ki,
            //p için spesifik bir pokemon olsun bu pokemonun özellikleri şöyledir: withmany.. hasforeignkey...
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
        }
    }
}
