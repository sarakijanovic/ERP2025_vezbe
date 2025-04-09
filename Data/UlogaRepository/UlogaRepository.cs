using AutoMapper;
using ERP2024.Models.Entities;

namespace ERP2024.Data.UlogaRepository
{
    public class UlogaRepository : IUlogaRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;

        public UlogaRepository(IMapper mapper, DatabaseContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }
        public Uloga CreateUloga(Uloga uloga)
        {
            var createdUloga = this.context.uloga.Add(uloga);
            this.context.SaveChanges();
            return mapper.Map<Uloga>(createdUloga.Entity);
        }

        public void DeleteUloga(Guid ulogaID)
        {
            var uloga = GetUlogaById(ulogaID);
            this.context.Remove(uloga);
            this.context.SaveChanges();
        }

        public Uloga GetUlogaById(Guid ulogaID)
        {
            return this.context.uloga.FirstOrDefault(e => e.ulogaID == ulogaID);
        }

        public List<Uloga> GetUloge()
        {
            return this.context.uloga.ToList();
        }

        public Uloga UpdateUloga(Uloga uloga)
        {
            try
            {
                var existingUloga = this.context.uloga.FirstOrDefault(e => e.ulogaID == uloga.ulogaID);

                if (existingUloga != null)
                {
                    existingUloga.nazivUloge = uloga.nazivUloge;
                    existingUloga.privilegije = uloga.privilegije;
                    this.context.SaveChanges();

                    return existingUloga;
                }
                else
                {
                    throw new KeyNotFoundException($"Uloga with ID {uloga.ulogaID} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating uloga.", ex);
            }
        }
    }
}
