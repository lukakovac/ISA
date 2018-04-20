using System.Collections;
using System.Collections.Generic;

namespace ISA.DataAccess.Models
{
    public class FunZone : BaseEntity<int>
    {
        public int? TheaterId { get; set; }
        public Theater Theater { get; set; }

        public int? CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        
        public ICollection<ThematicProps> ThematicProps { get; set; }
    }
}
