using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations;

namespace ERP2024.Models.Entities
{
    public class Porudzbina
    {
        [Key]
        public Guid porudzbinaID { get; set; }
        public bool dostavljena { get; set; }
        public DateOnly datumPorudzbine { get; set; }
        public double iznos {  get; set; }
        public Guid zaposleniID { get; set; }
        public Guid klijentID {  get; set; }    
    }
}
