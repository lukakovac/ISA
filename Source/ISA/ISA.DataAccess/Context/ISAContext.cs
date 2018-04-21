using ISA.DataAccess.Models;
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

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }

        public DbSet<Bid> Bids { get; set; }

        public DbSet<FunZone> FunZone { get; set; }

        public DbSet<ThematicProps> ThematicProps { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Repertoire> Repertoires { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                .HasIndex(x => x.EmailAddress)
                .IsUnique();

            modelBuilder.Entity<FriendRequest>()
                .HasOne(x => x.Sender)
                .WithMany(x => x.SentRequests)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequest>()
                .HasOne(x => x.Receiver)
                .WithMany(x => x.ReceivedRequests)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserProfile>()
                .HasMany(x => x.SentRequests)
                .WithOne(b => b.Sender)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserProfile>()
                .HasMany(x => x.ReceivedRequests)
                .WithOne(b => b.Receiver)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserProfile>()
                .HasMany(x => x.Reservations)
                .WithOne(x => x.UserProfile);


            modelBuilder.Entity<Bid>()
                .HasOne(x => x.User)
                .WithMany(x => x.Bids)
                .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<ThematicProps>()
                .HasOne(x => x.Publisher)
                .WithMany(x => x.ThematicProps);

            modelBuilder.Entity<ThematicProps>()
                .HasMany(x => x.Reservations)
                .WithOne(x => x.ThematicProp);

            base.OnModelCreating(modelBuilder);
        }

    }
}
