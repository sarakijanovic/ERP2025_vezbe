using System.ComponentModel.DataAnnotations;

namespace ERP2024.Models.Entities
{
    public class Uloga
    {
        [Key]
        public Guid ulogaID { get; set; }
        public string nazivUloge { get; set; }
        public string privilegije { get; set; }
    }
}
