using ERP2024.Models.Entities;

namespace ERP2024.Data.AparatPorudzbinaRepository
{
    public interface IAparatPorudzbinaRepository
    {
        List<AparatPorudzbina> GetAparatporudzbina();
        AparatPorudzbina GetAparatPorudzbinaById(Guid aparatID, Guid porudzbinaID);
        AparatPorudzbina CreateAparatporudzbina(AparatPorudzbina aparatPorudzbina);
        AparatPorudzbina UpdateAparatPorudzbina(AparatPorudzbina aparatPorudzbina);
        void DeleteAparatPorudzbina(Guid aparatID, Guid porudzbinaID);
    }
}
