using ERP2024.Data.KlijentRepository;
using ERP2024.Data.UlogaRepository;
using ERP2024.Data.ZaposleniRepository;
using ERP2024.Helpers;
using ERP2024.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP2024.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class AuthController : Controller
    {
        private readonly IZaposleniRepository zaposleniRepository;
        private readonly IKlijentRepository klijentRepository;
        private readonly IUlogaRepository ulogaRepository;
        private readonly IAuthHelper authHelper;
        public AuthController(IZaposleniRepository zaposleniRepository, IKlijentRepository klijentRepository, IUlogaRepository ulogaRepository, IAuthHelper authHelper)
        {
            this.zaposleniRepository = zaposleniRepository;
            this.klijentRepository = klijentRepository;
            this.ulogaRepository = ulogaRepository;
            this.authHelper = authHelper;
        }

        [HttpPost("zaposleni")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AuthenticateZaposleni([FromBody] AuthCreds creds)
        {
            if (authHelper.AuthenticateCreds(creds, true))
            {
                Guid ulogaID = zaposleniRepository.GetZaposleniByUsername(creds.korisnickoIme).ulogaID;
                string nazivUloge = ulogaRepository.GetUlogaById(ulogaID).nazivUloge;
                var tokenString = authHelper.GenerateJwt(creds, nazivUloge);
                return Ok(new { token = tokenString, uloga = nazivUloge });
            }

            return Unauthorized();
        }

        [HttpPost("klijent")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AuthenticateKlijent([FromBody] AuthCreds creds)
        {
            if (authHelper.AuthenticateCreds(creds, false))
            {
                var tokenString = authHelper.GenerateJwt(creds, "Customer");
                return Ok(new { token = tokenString, uloga = "Customer" });
            }

            return Unauthorized();
        }
    }
    
}
