using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP2024.Models.Entities
{
    [Table("APARAT_PORUDZBINA")]
    [PrimaryKey(nameof(aparatID), nameof(porudzbinaID))]
    public class AparatPorudzbina
    {
        public Guid aparatID {  get; set; }
        public Guid porudzbinaID { get; set; }
        public int kolicina { get; set; }
    }
}
