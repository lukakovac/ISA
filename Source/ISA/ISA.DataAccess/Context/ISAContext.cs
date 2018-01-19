using ISA.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISA.DataAccess.Context
{
    public class ISAContext : DbContext
    {
        public ISAContext() : base()
        { }

        public ISAContext(DbContextOptions<ISAContext> options) :
            base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Cinema> Cinemas { get; set; }
    }
}
