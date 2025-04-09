using ERP2024.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace ERP2024.Data
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly static int iterations = 1000;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
        }

        public DbSet<Zaposleni> zaposleni { get; set; }
        public DbSet<Uloga> uloga { get; set; }
        public DbSet<AparatZaVodu> aparatZaVodu { get; set; }
        public DbSet<TipAparata> tipAparata { get; set; }
        public DbSet<Porudzbina> porudzbina { get; set; }
        public DbSet<AparatPorudzbina> aparatPorudzbina { get; set; }
        public DbSet<Klijent> klijent { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Porudzbina>()
                .ToTable(tb => tb.HasTrigger("OdgovoranZaPorudzbinu"));
            modelBuilder.Entity<AparatPorudzbina>()
                .ToTable(tb => tb.HasTrigger("KolicinaNaStanjuTrigger"));
        }

        private Tuple<string, string> HashPassword(string password)
        {
            var sBytes = new byte[password.Length];
            new RNGCryptoServiceProvider().GetNonZeroBytes(sBytes);
            var salt = Convert.ToBase64String(sBytes);

            var derivedBytes = new Rfc2898DeriveBytes(password, sBytes, iterations);

            return new Tuple<string, string>
            (
                Convert.ToBase64String(derivedBytes.GetBytes(256)),
                salt
            );
        }
    }
}
