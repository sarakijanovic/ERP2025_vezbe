namespace ERP2024.Models.DTOs.Porudzbina
{
    public class PorudzbinaDto
    {
        public Guid porudzbinaID { get; set; }
        public bool dostavljena { get; set; }
        public DateOnly datumPorudzbine { get; set; }
        public float iznos { get; set; }
        public Guid zaposleniID { get; set; }
        public Guid klijentID { get; set; }
    }
}
