using ISA.DataAccess.Context;
using ISA.DataAccess.Models;

namespace ISA.DataAccess.Repository
{
    public class CinemaRepo : RepositoryBase<Cinema, int>
    {
        public CinemaRepo(ISAContext context) : base(context) { }
    }
}
