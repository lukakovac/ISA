using ISA.DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ISA.DataAccess.Context
{
    public class ISAContext : DbContext
    {
        public ISAContext() : base()
        { }

        public ISAContext(DbContextOptions<ISAContext> options)
            : base(options)
        {
            var isCreated = Database.EnsureCreated();
        }

        public DbSet<Cinema> Cinemas { get; set; }

        public DbSet<Bid> Bids { get; set; }

        public DbSet<FunZone> FunZone { get; set; }

        public DbSet<ThematicProps> ThematicProps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Repertoire> Repertoires { get; set; }

    }
}
