using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ISA.DataAccess.Models
{
    public class ThematicProps : BaseEntity<int>
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public UserProfile Publisher { get; set; }

        public DateTime ValidTo { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    
        public FunZone FunZone { get; set; }

        public string Image { get; set; }
    }
}