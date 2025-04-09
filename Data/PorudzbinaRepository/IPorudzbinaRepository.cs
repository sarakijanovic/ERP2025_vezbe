using ERP2024.Models.Entities;

namespace ERP2024.Data.PorudzbinaRepository
{
    public interface IPorudzbinaRepository
    {
        List<Porudzbina> GetPorudzbine();
        Porudzbina GetPorudzbinaById(Guid porudzbinaID);
        Porudzbina CreatePorudzbina(Porudzbina porudzbina);
        Porudzbina UpdatePorudzbina(Porudzbina porudzbina);
        void DeletePorudzbina(Guid porudzbinaID);
    }
}
