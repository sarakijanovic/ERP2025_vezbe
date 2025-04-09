using AutoMapper;
using ERP2024.Models.Entities;

namespace ERP2024.Data.TipAparataRepository
{
    public class TipAparataRepository : ITipAparataRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;

        public TipAparataRepository(IMapper mapper, DatabaseContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public TipAparata CreateTipAparata(TipAparata tipAparata)
        {
            var createdTipAparata = this.context.tipAparata.Add(tipAparata);
            this.context.SaveChanges();
            return mapper.Map<TipAparata>(createdTipAparata.Entity);
        }

        public void DeleteTipAparata(Guid tipAparataID)
        {
            var tipAparata = GetTipAparataById(tipAparataID);
            this.context.Remove(tipAparata);
            this.context.SaveChanges();
        }

        public List<TipAparata> GetTipAparata()
        {
            return this.context.tipAparata.ToList();
        }

        public TipAparata GetTipAparataById(Guid tipAparataID)
        {
            return this.context.tipAparata.FirstOrDefault(e => e.tipAparataID == tipAparataID);
        }

        public TipAparata UpdateTipAparata(TipAparata tipAparata)
        {
            try
            {
                var existingTipAparata = this.context.tipAparata.FirstOrDefault(e => e.tipAparataID == tipAparata.tipAparataID);

                if (existingTipAparata != null)
                {
                    existingTipAparata.nazivTipa = tipAparata.nazivTipa;
                    existingTipAparata.mililitraza = tipAparata.mililitraza;
                    existingTipAparata.vrucaVoda = tipAparata.vrucaVoda;
                    existingTipAparata.dodatneInfo = tipAparata.dodatneInfo;
                    this.context.SaveChanges();

                    return existingTipAparata;
                }
                else
                {
                    throw new KeyNotFoundException($"Tip aparata with ID {tipAparata.tipAparataID} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating tip aparata.", ex);
            }
        }
    }
}
