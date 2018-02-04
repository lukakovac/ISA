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
            Database.EnsureCreated();
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
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
        }
    }
}
