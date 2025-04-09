namespace ERP2024.Models.DTOs.Zaposleni
{
    public class ZaposleniDto
    {
        public Guid zaposleniID { get; set; }
        public string imeZaposlenog { get; set; }
        public string prezimeZaposlenog { get; set; }
        public string JMBG { get; set; }
        public string korisnickoImeZaposlenog { get; set; }
        public string emailZaposlenog { get; set; }
        public Guid ulogaID { get; set; }

    }
}
