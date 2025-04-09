using ERP2024.Models.Entities;

namespace ERP2024.Data.AparatZaVoduRepository
{
    public interface IAparatZaVoduRepository
    {
        List<AparatZaVodu> GetAparati();
        AparatZaVodu GetAparatById(Guid aparatID);
        AparatZaVodu CreateAparat(AparatZaVodu aparat);
        AparatZaVodu UpdateAparat(AparatZaVodu aparat);
        void DeleteAparat(Guid aparatID);
    }
}
