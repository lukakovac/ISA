using ISA.Common.Extensions;
using ISA.DataAccess.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISA.DataAccess.Models
{
    public class Projection : BaseEntity<int>
    {
        public TimeSpan Duration { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Column("Type")]
        public string ProjectionTypeString
        {
            get => Type.ToString();
            set => Type = value.ParseEnum<ProjectionTypeEnum>();
        }

        [NotMapped]
        public ProjectionTypeEnum Type { get; set; }
    }

}
