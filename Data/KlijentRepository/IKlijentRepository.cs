using ERP2024.Models.DTOs.Klijent;
using ERP2024.Models.Entities;

namespace ERP2024.Data.KlijentRepository
{
    public interface IKlijentRepository
    {
        List<Klijent> GetKlijent();
        Klijent GetKlijentById(Guid klijentID);
        Klijent CreateKlijent(KlijentCreationDto klijent);
        Klijent UpdateKlijent(KlijentUpdateDto klijent);
        void DeleteKlijent(Guid klijentID);
        bool KlijentWithCredentialsExists(string korisnickoIme, string lozinka);
        Klijent GetKlijentByUsername(string korisnickoIme);

    }
}
