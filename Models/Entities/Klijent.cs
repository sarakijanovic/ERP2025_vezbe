using System.ComponentModel.DataAnnotations;

namespace ERP2024.Models.Entities
{
    public class Klijent
    {
        [Key]
        public Guid klijentID { get; set; }
        public string imeKlijenta { get; set; }
        public string prezimeKlijenta { get; set; }
        public DateOnly datumRodjenja { get; set; }
        public string adresa { get; set; }
        public string kontakt { get; set; }
        public string korisnickoImeKlijenta { get; set; }
        public string emailKlijenta { get; set; }
        public byte[] lozinkaKlijentaHashed { get; set; }
        public byte[] saltKlijenta { get; set; }
    }
}
