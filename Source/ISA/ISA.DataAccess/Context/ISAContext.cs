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

            modelBuilder.Entity<Cinema>()
                .HasOne(p => p.FunZone)
                .WithOne(i => i.Cinema)
                .HasForeignKey<FunZone>(b => b.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Theater>()
                .HasOne(p => p.FunZone)
                .WithOne(i => i.Theater)
                .HasForeignKey<FunZone>(b => b.TheaterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ThematicProps>()
                .HasOne(p => p.FunZone)
                .WithMany(b => b.ThematicProps);
        }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Repertoire> Repertoires { get; set; }

    }
}
