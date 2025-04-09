using AutoMapper;
using ERP2024.Models.DTOs.Klijent;
using ERP2024.Models.Entities;
using System.Security.Cryptography;

namespace ERP2024.Data.KlijentRepository
{
    public class KlijentRepository : IKlijentRepository
    {
        public readonly DatabaseContext context;
        public readonly IMapper mapper;
        private readonly static int iterations = 1000;

        public KlijentRepository(IMapper mapper, DatabaseContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }
        public Klijent CreateKlijent(KlijentCreationDto klijent)
        {
            Klijent klijentEntity = mapper.Map<Klijent>(klijent);
            klijentEntity.klijentID = Guid.NewGuid();
            var lozinkaKlijentaHashed = HashPassword(klijent.lozinkaKlijenta);
            klijentEntity.lozinkaKlijentaHashed = Convert.FromBase64String(lozinkaKlijentaHashed.Item1);
            klijentEntity.saltKlijenta = Convert.FromBase64String(lozinkaKlijentaHashed.Item2);
            var createdKlijent = this.context.klijent.Add(klijentEntity);
            this.context.SaveChanges();
            return mapper.Map<Klijent>(createdKlijent.Entity);
        }

        public void DeleteKlijent(Guid klijentID)
        {
            var klijent = GetKlijentById(klijentID);
            this.context.Remove(klijent);
            this.context.SaveChanges();
        }

        public List<Klijent> GetKlijent()
        {
            return this.context.klijent.ToList();
        }

        public Klijent GetKlijentById(Guid klijentID)
        {
            return this.context.klijent.FirstOrDefault(e => e.klijentID == klijentID);
        }

        public Klijent GetKlijentByUsername(string korisnickoIme)
        {
            return context.klijent.FirstOrDefault(k => k.korisnickoImeKlijenta == korisnickoIme);
        }

        public bool KlijentWithCredentialsExists(string korisnickoIme, string lozinka)
        {
            Klijent klijent = context.klijent.FirstOrDefault(k => k.korisnickoImeKlijenta == korisnickoIme);

            if (klijent == null)
            {
                return false;
            }
            if (VerifyPassword(lozinka, Convert.ToBase64String(klijent.lozinkaKlijentaHashed), klijent.saltKlijenta))
            {
                return true;
            }
            return false;
        }

        public Klijent UpdateKlijent(KlijentUpdateDto klijent)
        {
            try
            {
                var existingKlijent = this.context.klijent.FirstOrDefault(e => e.klijentID == klijent.klijentID);

                if (existingKlijent != null)
                {
                    existingKlijent.imeKlijenta = klijent.imeKlijenta;
                    existingKlijent.prezimeKlijenta = klijent.prezimeKlijenta;
                    existingKlijent.datumRodjenja = DateOnly.Parse(klijent.datumRodjenja);
                    existingKlijent.adresa = klijent.adresa;
                    existingKlijent.kontakt = klijent.kontakt;
                    existingKlijent.korisnickoImeKlijenta = klijent.korisnickoImeKlijenta;
                    existingKlijent.emailKlijenta = klijent.emailKlijenta;

                    var novaLozinkaHashed = HashPassword(klijent.lozinkaKlijenta);
                    existingKlijent.lozinkaKlijentaHashed = Convert.FromBase64String(novaLozinkaHashed.Item1);
                    existingKlijent.saltKlijenta = Convert.FromBase64String(novaLozinkaHashed.Item2);

                    this.context.SaveChanges();

                    return existingKlijent;
                }
                else
                {
                    throw new KeyNotFoundException($"Klijent with ID {klijent.klijentID} not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating klijent.", ex);
            }
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
