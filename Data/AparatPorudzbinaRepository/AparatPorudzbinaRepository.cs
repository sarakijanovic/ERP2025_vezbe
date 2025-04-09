using AutoMapper;
using ERP2024.Models.Entities;

namespace ERP2024.Data.AparatPorudzbinaRepository
{
    public class AparatPorudzbinaRepository : IAparatPorudzbinaRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;

        public AparatPorudzbinaRepository(IMapper mapper, DatabaseContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }
        public AparatPorudzbina CreateAparatporudzbina(AparatPorudzbina aparatPorudzbina)
        {
            var createdAparatPorudzbina = this.context.aparatPorudzbina.Add(aparatPorudzbina);
            this.context.SaveChanges();
            return mapper.Map<AparatPorudzbina>(createdAparatPorudzbina.Entity);
        }

        public void DeleteAparatPorudzbina(Guid aparatID, Guid porudzbinaID)
        {
            var aparatPorudzbina = GetAparatPorudzbinaById(aparatID, porudzbinaID);
            this.context.Remove(aparatPorudzbina);
            this.context.SaveChanges();
        }

        public List<AparatPorudzbina> GetAparatporudzbina()
        {
            return this.context.aparatPorudzbina.ToList();
        }

        public AparatPorudzbina GetAparatPorudzbinaById(Guid aparatID, Guid porudzbinaID)
        {
            return this.context.aparatPorudzbina.FirstOrDefault(e => (e.aparatID == aparatID && e.porudzbinaID == porudzbinaID));
        }

        public AparatPorudzbina UpdateAparatPorudzbina(AparatPorudzbina aparatPorudzbina)
        {
            try
            {
                var existingAparatPorudzbina = this.context.aparatPorudzbina.FirstOrDefault(e => (e.aparatID == aparatPorudzbina.aparatID && e.porudzbinaID == aparatPorudzbina.porudzbinaID));

                if (existingAparatPorudzbina != null)
                {
                    existingAparatPorudzbina.kolicina = aparatPorudzbina.kolicina;
                    this.context.SaveChanges();

                    return existingAparatPorudzbina;
                }
                else
                {
                    throw new KeyNotFoundException($"AparatPordzbina with IDs {aparatPorudzbina.aparatID} and {aparatPorudzbina.porudzbinaID} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating AparatPorudzbina.", ex);
            }
        }
    }
}
