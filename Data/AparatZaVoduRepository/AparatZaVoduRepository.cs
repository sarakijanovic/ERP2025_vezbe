using AutoMapper;
using ERP2024.Models.Entities;

namespace ERP2024.Data.AparatZaVoduRepository
{
    public class AparatZaVoduRepository : IAparatZaVoduRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;

        public AparatZaVoduRepository(IMapper mapper, DatabaseContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public AparatZaVodu CreateAparat(AparatZaVodu aparat)
        {
            var createdAparat = this.context.aparatZaVodu.Add(aparat);
            this.context.SaveChanges();
            return mapper.Map<AparatZaVodu>(createdAparat.Entity);
        }

        public void DeleteAparat(Guid aparatID)
        {
            var aparat = GetAparatById(aparatID);
            this.context.Remove(aparat);
            this.context.SaveChanges();
        }

        public AparatZaVodu GetAparatById(Guid aparatID)
        {
            return this.context.aparatZaVodu.FirstOrDefault(e => e.aparatID == aparatID);
        }

        public List<AparatZaVodu> GetAparati()
        {
            return this.context.aparatZaVodu.ToList();
        }

        public AparatZaVodu UpdateAparat(AparatZaVodu aparat)
        {
            try
            {
                var existingAparat = this.context.aparatZaVodu.FirstOrDefault(e => e.aparatID == aparat.aparatID);

                if (existingAparat != null)
                {
                    existingAparat.model = aparat.model;
                    existingAparat.proizvodjac = aparat.proizvodjac;
                    existingAparat.slikaURL = aparat.slikaURL;
                    existingAparat.cena = aparat.cena;
                    existingAparat.opis = aparat.opis;
                    existingAparat.kolicinaNaStanju = aparat.kolicinaNaStanju;
                    existingAparat.tipAparataID = aparat.tipAparataID;
                    this.context.SaveChanges();

                    return existingAparat;
                }
                else
                {
                    throw new KeyNotFoundException($"Aparat with ID {aparat.aparatID} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating aparat.", ex);
            }
        }
    }
}
