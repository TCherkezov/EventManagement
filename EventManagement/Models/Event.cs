using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagement.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public Category Category { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TicketPrice { get; set; }
    }
}
