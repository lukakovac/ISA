using ISA.Common.Extensions;
using ISA.DataAccess.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.DataAccess.Models
{
    public class Cinema : BaseEntity<int>
    {        
        public string Name { get; set; }

        public string Address { get; set; }

        #region ENUM PATTERN
        [Column("Type")]
        public string TypeString
        {
            get => Type.ToString();
            set => Type = value.ParseEnum<CinemaTypeEnum>();
        }

        public FunZone FunZone { get; set; }

        [NotMapped]
        public CinemaTypeEnum Type { get; set; }
        #endregion

        //public virtual List<ProjectionHall> ProjectionHalls { get; set; }

        public virtual List<Repertoire> Repertoires { get; set; }
    }

    public class ProjectionHall { }

    public class Repertoire : BaseEntity<int>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<Projection> Projections { get; set; }
    }
}
