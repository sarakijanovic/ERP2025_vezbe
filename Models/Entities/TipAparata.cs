using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP2024.Models.Entities
{
    [Table("TIP_APARATA")]
    public class TipAparata
    {
        [Key]
        public Guid tipAparataID { get; set; }
        public string nazivTipa { get; set; }
        public int mililitraza { get; set; }
        public bool vrucaVoda { get; set; }
        public string dodatneInfo { get; set; }
    }
}
