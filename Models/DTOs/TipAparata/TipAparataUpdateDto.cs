namespace ERP2024.Models.DTOs.TipAparata
{
    public class TipAparataUpdateDto
    {
        public Guid tipAparataID { get; set; }
        public string nazivTipa { get; set; }
        public int mililitraza { get; set; }
        public bool vrucaVoda { get; set; }
        public string dodatneInfo { get; set; }
    }
}
