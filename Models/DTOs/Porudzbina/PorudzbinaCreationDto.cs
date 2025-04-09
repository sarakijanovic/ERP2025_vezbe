namespace ERP2024.Models.DTOs.Porudzbina
{
    public class PorudzbinaCreationDto
    {
        public bool dostavljena { get; set; }
        public string datumPorudzbine { get; set; }
        public float iznos { get; set; }
        public Guid zaposleniID { get; set; }
        public Guid klijentID { get; set; }
    }
}
