using System;

namespace ISA.DataAccess.Models
{
    public class Reservation : BaseEntity<int>
    {
        public DateTime ReservationValidTo { get; set; }

        public ThematicProps ThematicProp { get; set; }

        public Projection Projection { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
