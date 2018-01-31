using System.ComponentModel.DataAnnotations;

namespace ISA.DataAccess.Models
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
