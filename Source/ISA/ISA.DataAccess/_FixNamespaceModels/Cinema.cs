using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISA.Models
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public IEnumerable<Projection> Projections { get; set; }

        public IEnumerable<Seat> Seats { get; set; }
    }
}
