﻿namespace ISA.DataAccess.Models
{
    public class Theater : BaseEntity<int>
    {
        public string Name { get; set; }

        public FunZone FunZone { get; set; }
    }
}
