using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CollectibleCardsTradingShopProject.Models;
using System.Reflection.Emit;

namespace CollectibleCardsTradingShopProject.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Franchise> Franchises { get; set; }
        public virtual DbSet<Rarity> Rarities { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<LotCardStatus> LotCardStatuses { get; set; }
        public virtual DbSet<CardInLot> CardInLots { get; set; }
        public virtual DbSet<Lot> Lots { get; set; }
        public virtual DbSet<UserCard> UserCards { get; set; }
        public virtual DbSet<UserLot> UserLots { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema("identity");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
