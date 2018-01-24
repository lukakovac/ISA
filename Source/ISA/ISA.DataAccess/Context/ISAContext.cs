using ISA.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ISA.DataAccess.Context
{
    public class ISAContext : DbContext
    {
        public ISAContext() : base()
        { }

        public ISAContext(DbContextOptions<ISAContext> options) :
            base(options)
        {
            var isCreated = Database.EnsureCreated();
        }

        public DbSet<Cinema> Cinemas { get; set; }
    }
}
