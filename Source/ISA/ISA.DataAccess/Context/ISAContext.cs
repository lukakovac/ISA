using ISA.DataAccess.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ISA.DataAccess.Context
{
    public class ISAContext : DbContext
    {
        public ISAContext() : base()
        { }

        public ISAContext(DbContextOptions<ISAContext> options, IHostingEnvironment env)
            : base(options)
        {
            if (!env.IsProduction())
            {
                Database.EnsureDeleted();
            }
            Database.EnsureCreated();
        }

        //public DbSet<CinemaType> CinemaTypes { get; set; }

        public DbSet<Cinema> Cinemas { get; set; }
    }
}
