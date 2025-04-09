using AutoMapper;
using ERP2024.Data.ZaposleniRepository;
using ERP2024.Models.DTOs.Zaposleni;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP2024.Controllers
{
    [ApiController]
    [Route("api/zaposleni")]
    public class ZaposleniController : Controller
    {
        private readonly IZaposleniRepository zaposleniRepository;
        private readonly IMapper mapper;

        public ZaposleniController(IMapper mapper, IZaposleniRepository zaposleniRepository)
        {
            this.mapper = mapper;
            this.zaposleniRepository = zaposleniRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<ZaposleniDto>> GetZaposleni(int page = 1, int pageSize = 10)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            /*var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin"));

            if (roleClaim == null)
            {
                return Forbid();
            }*/
            var zaposleni = zaposleniRepository.GetZaposleni();

            if (zaposleni == null || zaposleni.Count == 0)
            {
                return NoContent();
            }

            List<ZaposleniDto> zaposleniDto = new List<ZaposleniDto>();
            foreach (var zap in zaposleni)
            {
                zaposleniDto.Add(mapper.Map<ZaposleniDto>(zap));
            }

            var totalCount = zaposleniDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = zaposleniDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("id/{zaposleniID}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Admin")]
        public ActionResult<ZaposleniDto> GetZaposleniByID(Guid zaposleniID)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            /*var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin"));

            if (roleClaim == null)
            {
                return Forbid();
            }*/
            var zaposleni = zaposleniRepository.GetZaposleniById(zaposleniID);

            if (zaposleni == null)
            {
                return NotFound();
            }

            return mapper.Map<ZaposleniDto>(zaposleni);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("{korisnickoImeZapolsenog}")]
        public ActionResult<ZaposleniDto> GetZaposleniByUsername(string korisnickoImeZapolsenog)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var zaposleni = zaposleniRepository.GetZaposleniByUsername(korisnickoImeZapolsenog);

            if (zaposleni == null)
            {
                return NotFound();
            }

            return mapper.Map<ZaposleniDto>(zaposleni);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<ZaposleniDto> CreateZaposleni([FromBody] ZaposleniCreationDto zaposleniCreationDto)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin"));

                if (roleClaim == null)
                {
                    return Forbid();
                }
                Zaposleni createdZaposleni = zaposleniRepository.CreateZaposleni(zaposleniCreationDto);
                return mapper.Map<ZaposleniDto>(createdZaposleni);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{zaposleniID}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteZaposleni(Guid zaposleniID)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin"));

                if (roleClaim == null)
                {
                    return Forbid();
                }
                var zaposleni = zaposleniRepository.GetZaposleniById(zaposleniID);
                if (zaposleni == null)
                {
                    return NotFound();
                }

                zaposleniRepository.DeleteZaposleni(zaposleniID);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }
        [HttpPut]
        //[Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ZaposleniDto> UpdateZaposleni(ZaposleniUpdateDto zaposleniUpdateDto)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin"));

                if (roleClaim == null)
                {
                    return Forbid();
                }
                var updatedZaposleni = zaposleniRepository.UpdateZaposleni(zaposleniUpdateDto);

                ZaposleniDto updatedZaposleniDto = mapper.Map<ZaposleniDto>(updatedZaposleni);

                return Ok(updatedZaposleniDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update Error");
            }
        }
    }
}
