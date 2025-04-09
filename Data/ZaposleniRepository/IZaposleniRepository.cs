using ERP2024.Models.DTOs.Zaposleni;
using ERP2024.Models.Entities;

namespace ERP2024.Data.ZaposleniRepository
{
    public interface IZaposleniRepository
    {
        List<Zaposleni> GetZaposleni();
        Zaposleni GetZaposleniById(Guid zaposleniID);
        Zaposleni CreateZaposleni(ZaposleniCreationDto zaposleni);
        Zaposleni UpdateZaposleni(ZaposleniUpdateDto zaposleni);
        void DeleteZaposleni(Guid zaposleniID);
        void DeleteZaposleni(string korisnickoIme);
        bool ZaposleniWithCredentialsExists(string korisnickoIme, string lozinka);
        Zaposleni GetZaposleniByUsername(string korisnickoIme);
    }
}
