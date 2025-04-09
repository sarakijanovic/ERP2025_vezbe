using AutoMapper;
using ERP2024.Models.DTOs.Zaposleni;
using ERP2024.Models.Entities;
using System.Security.Cryptography;

namespace ERP2024.Data.ZaposleniRepository
{
    public class ZaposleniRepository : IZaposleniRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;
        private readonly static int iterations = 1000;
        public ZaposleniRepository(IMapper mapper, DatabaseContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }
        public List<Zaposleni> GetZaposleni()
        {
            return this.context.zaposleni.ToList();
        }
        public Zaposleni GetZaposleniById(Guid zaposleniID)
        {
            return this.context.zaposleni.FirstOrDefault(e => e.zaposleniID == zaposleniID);
        }
        public Zaposleni CreateZaposleni(ZaposleniCreationDto zaposleni)
        {
            Zaposleni zaposleniEntity = mapper.Map<Zaposleni>(zaposleni);
            zaposleniEntity.zaposleniID = Guid.NewGuid();
            var lozinkaZaposlenogHashed = HashPassword(zaposleni.lozinkaZaposlenog);
            zaposleniEntity.lozinkaZaposlenogHashed = Convert.FromBase64String(lozinkaZaposlenogHashed.Item1);
            zaposleniEntity.saltZaposlenog = Convert.FromBase64String(lozinkaZaposlenogHashed.Item2);
            var createdZaposleni = this.context.zaposleni.Add(zaposleniEntity);
            this.context.SaveChanges();
            return mapper.Map<Zaposleni>(createdZaposleni.Entity);
        }

        public void DeleteZaposleni(Guid zaposleniID)
        {
            var zaposleni = GetZaposleniById(zaposleniID);
            this.context.Remove(zaposleni);
            this.context.SaveChanges();
        }
        public void DeleteZaposleni(string korisnickoIme)
        {
            try
            {
                var zaposleni = context.zaposleni.FirstOrDefault(e => e.korisnickoImeZaposlenog == korisnickoIme);

                if (zaposleni != null)
                {

                    context.zaposleni.Remove(zaposleni);
                    context.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException($"Zaposleni with username {korisnickoIme} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting zaposleni.", ex);
            }
        }
        public Zaposleni UpdateZaposleni(ZaposleniUpdateDto zaposleni)
        {
            try
            {
                var existingZaposleni = this.context.zaposleni.FirstOrDefault(e => e.zaposleniID == zaposleni.zaposleniID);

                if (existingZaposleni != null)
                {
                    existingZaposleni.imeZaposlenog = existingZaposleni.imeZaposlenog;
                    existingZaposleni.prezimeZaposlenog = existingZaposleni.prezimeZaposlenog;
                    existingZaposleni.JMBG = existingZaposleni.JMBG;
                    existingZaposleni.korisnickoImeZaposlenog = existingZaposleni.korisnickoImeZaposlenog;
                    existingZaposleni.emailZaposlenog = existingZaposleni.emailZaposlenog;
                    existingZaposleni.ulogaID = existingZaposleni.ulogaID;

                    var novaLozinkaHashed = HashPassword(zaposleni.lozinkaZaposlenog);
                    existingZaposleni.lozinkaZaposlenogHashed = Convert.FromBase64String(novaLozinkaHashed.Item1);
                    existingZaposleni.saltZaposlenog = Convert.FromBase64String(novaLozinkaHashed.Item2);

                    this.context.SaveChanges();

                    return existingZaposleni;
                }
                else
                {
                    throw new KeyNotFoundException($"Zaposleni with ID {zaposleni.zaposleniID} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating zaposleni.", ex);
            }
        }

        public bool ZaposleniWithCredentialsExists(string korisnickoIme, string lozinka)
        {
            Zaposleni zaposleni = context.zaposleni.FirstOrDefault(z => z.korisnickoImeZaposlenog == korisnickoIme);

            if (zaposleni == null)
            {
                return false;
            }
            if (VerifyPassword(lozinka, Convert.ToBase64String(zaposleni.lozinkaZaposlenogHashed), zaposleni.saltZaposlenog))
            {
                return true;
            }
            return false;
        }

        public Zaposleni GetZaposleniByUsername(string korisnickoIme)
        { 
            return context.zaposleni.FirstOrDefault(z => z.korisnickoImeZaposlenog == korisnickoIme);
        }

        //Pomocne metode

        public bool VerifyPassword(string lozinka, string lozinkaHashed, byte[] salt)
        {
            var saltBytes = salt;
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(lozinka, saltBytes, iterations);
            if (Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == lozinkaHashed)
            {
                return true;
            }
            return false;
        }
        private Tuple<string, string> HashPassword(string lozinka)
        {
            var sBytes = new byte[lozinka.Length];
            new RNGCryptoServiceProvider().GetNonZeroBytes(sBytes);
            var salt = Convert.ToBase64String(sBytes);

            var derivedBytes = new Rfc2898DeriveBytes(lozinka, sBytes, iterations);

            return new Tuple<string, string>
            (
                Convert.ToBase64String(derivedBytes.GetBytes(256)),
                salt
            );
        }
    }
}
