using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ERP2024.Models.Entities
{
    [Table("APARAT_ZA_VODU")]
    public class AparatZaVodu
    {
        [Key]
        public Guid aparatID {  get; set; }
        public string model {  get; set; }
        public string proizvodjac {  get; set; }
        public string opis {  get; set; }
        public string slikaURL { get; set; }
        public double cena { get; set; }
        public int kolicinaNaStanju { get; set; }
        public Guid tipAparataID { get; set; }
    }
}
