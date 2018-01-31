using System.Collections;
using System.Collections.Generic;

namespace ISA.DataAccess.Models
{
    public class FunZone : BaseEntity<int>
    {
        public Theater Theater { get; set; }

        public Cinema Cinema { get; set; }
        
        public virtual ICollection<ThematicProps> ThematicProps { get; set; }
    }
}
