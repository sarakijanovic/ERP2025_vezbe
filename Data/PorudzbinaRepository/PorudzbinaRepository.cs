using AutoMapper;
using ERP2024.Models.Entities;

namespace ERP2024.Data.PorudzbinaRepository
{
    public class PorudzbinaRepository : IPorudzbinaRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;

        public PorudzbinaRepository(IMapper mapper, DatabaseContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }
        public Porudzbina CreatePorudzbina(Porudzbina porudzbina)
        {
            var createdPorudzbina = this.context.porudzbina.Add(porudzbina);
            this.context.SaveChanges();
            return mapper.Map<Porudzbina>(createdPorudzbina.Entity);
        }

        public void DeletePorudzbina(Guid porudzbinaID)
        {
            var porudzbina = GetPorudzbinaById(porudzbinaID);
            this.context.Remove(porudzbina);
            this.context.SaveChanges();
        }

        public Porudzbina GetPorudzbinaById(Guid porudzbinaID)
        {
            return this.context.porudzbina.FirstOrDefault(e => e.porudzbinaID == porudzbinaID);
        }

        public List<Porudzbina> GetPorudzbine()
        {
            return this.context.porudzbina.ToList();
        }

        public Porudzbina UpdatePorudzbina(Porudzbina porudzbina)
        {
            try
            {
                var existingPorudzbina = this.context.porudzbina.FirstOrDefault(e => e.porudzbinaID == porudzbina.porudzbinaID);

                if (existingPorudzbina != null)
                {
                    existingPorudzbina.dostavljena = porudzbina.dostavljena;
                    existingPorudzbina.datumPorudzbine = porudzbina.datumPorudzbine;
                    existingPorudzbina.iznos = porudzbina.iznos;
                    existingPorudzbina.klijentID = porudzbina.klijentID;
                    existingPorudzbina.zaposleniID = porudzbina.zaposleniID;
                    this.context.SaveChanges();

                    return existingPorudzbina;
                }
                else
                {
                    throw new KeyNotFoundException($"Porudzbina with ID {porudzbina.porudzbinaID} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating porudzbina.", ex);
            }
        }
    }
}
