using ERP2024.Models.Entities;

namespace ERP2024.Data.TipAparataRepository
{
    public interface ITipAparataRepository
    {
        List<TipAparata> GetTipAparata();
        TipAparata GetTipAparataById(Guid tipAparataID);
        TipAparata CreateTipAparata(TipAparata tipAparata);
        TipAparata UpdateTipAparata(TipAparata tipAparata);
        void DeleteTipAparata(Guid tipAparataID);
    }
}
