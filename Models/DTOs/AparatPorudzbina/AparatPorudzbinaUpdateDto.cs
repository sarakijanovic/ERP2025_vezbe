namespace ERP2024.Models.DTOs.AparatPorudzbina
{
    public class AparatPorudzbinaUpdateDto
    {
        public Guid aparatID { get; set; }
        public Guid porudzbinaID { get; set; }
        public int kolicina { get; set; }
    }
}
