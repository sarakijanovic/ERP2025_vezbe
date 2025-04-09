using ERP2024.Models.Entities;

namespace ERP2024.Data.UlogaRepository
{
    public interface IUlogaRepository
    {
        List<Uloga> GetUloge();
        Uloga GetUlogaById(Guid ulogaID);
        Uloga CreateUloga(Uloga uloga);
        Uloga UpdateUloga(Uloga uloga);
        void DeleteUloga(Guid ulogaID);
    }
}
