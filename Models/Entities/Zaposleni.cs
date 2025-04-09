using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP2024.Models.Entities
{
    public class Zaposleni
    {
        [Key]
        public Guid zaposleniID { get; set; }
        public string imeZaposlenog { get; set; }
        public string prezimeZaposlenog { get; set; }
        public string JMBG { get; set; }
        public string korisnickoImeZaposlenog { get; set; }
        public string emailZaposlenog { get; set; }
        public byte[] lozinkaZaposlenogHashed { get; set; }
        public byte[] saltZaposlenog { get; set; }
        public Guid ulogaID { get; set; }
    }
}
