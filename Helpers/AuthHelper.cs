using ERP2024.Data.KlijentRepository;
using ERP2024.Data.ZaposleniRepository;
using ERP2024.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ERP2024.Helpers
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IConfiguration configuration;
        private readonly IZaposleniRepository zaposleniRepository;
        private readonly IKlijentRepository klijentRepository;

        public AuthHelper(IConfiguration configuration, IZaposleniRepository zaposleniRepository, IKlijentRepository klijentRepository)
        {
            this.configuration = configuration;
            this.zaposleniRepository = zaposleniRepository;
            this.klijentRepository = klijentRepository;
        }
        public bool AuthenticateCreds(AuthCreds creds, bool isEmployee)
        {
            if (isEmployee && zaposleniRepository.ZaposleniWithCredentialsExists(creds.korisnickoIme, creds.lozinka))
            {
                return true;

            } else if (!isEmployee && klijentRepository.KlijentWithCredentialsExists(creds.korisnickoIme, creds.lozinka))
            {
                return true;
            }

            return false;
        }

        public string GenerateJwt(AuthCreds creds, string role)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, creds.korisnickoIme),
                    new Claim(ClaimTypes.Role, role)
                };

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                                             configuration["Jwt:Issuer"],
                                             claims,
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

