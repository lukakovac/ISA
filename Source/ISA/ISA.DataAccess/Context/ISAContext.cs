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
            Database.EnsureCreated();
        }

        //public DbSet<CinemaType> CinemaTypes { get; set; }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Repertoire> Repertoires { get; set; }

    }
}
