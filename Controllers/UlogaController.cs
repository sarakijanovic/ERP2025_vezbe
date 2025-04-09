using AutoMapper;
using ERP2024.Data.UlogaRepository;
using ERP2024.Models.DTOs.Uloga;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP2024.Controllers
{
    [ApiController]
    [Route("api/uloga")]
    public class UlogaController : Controller
    {
        private readonly IUlogaRepository ulogaRepository;
        private readonly IMapper mapper;

        public UlogaController(IMapper mapper, IUlogaRepository ulogaRepository)
        {
            this.mapper = mapper;
            this.ulogaRepository = ulogaRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<UlogaDto>> GetUloge(int page = 1, int pageSize = 10)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin"));

            /*if (roleClaim == null)
            {
                return Forbid();
            }*/
            var uloge = ulogaRepository.GetUloge();

            if (uloge == null || uloge.Count == 0)
            {
                return NoContent();
            }

            List<UlogaDto> ulogeDto = new List<UlogaDto>();
            foreach (var uloga in uloge)
            {
                ulogeDto.Add(mapper.Map<UlogaDto>(uloga));
            }

            var totalCount = ulogeDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = ulogeDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{ulogaID}")]
        //[Authorize(Roles = "Admin")]
        public ActionResult<UlogaDto> GetUlogaByID(Guid ulogaID)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin"));

            /*if (roleClaim == null)
            {
                return Forbid();
            }*/
            var uloga = ulogaRepository.GetUlogaById(ulogaID);

            if (uloga == null)
            {
                return NotFound();
            }

            return mapper.Map<UlogaDto>(uloga);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UlogaDto> CreateUloga([FromBody] UlogaCreationDto ulogaCreationDto)
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
                Uloga mappedUloga = mapper.Map<Uloga>(ulogaCreationDto);
                mappedUloga.ulogaID = Guid.NewGuid();
                Uloga createdUloga = ulogaRepository.CreateUloga(mappedUloga);
                return mapper.Map<UlogaDto>(createdUloga);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{ulogaID}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteUloga(Guid ulogaID)
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
                var uloga = ulogaRepository.GetUlogaById(ulogaID);
                if (uloga == null)
                {
                    return NotFound();
                }

                ulogaRepository.DeleteUloga(ulogaID);

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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UlogaDto> UpdateUloga(UlogaUpdateDto ulogaUpdateDto)
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
                Uloga mappedUloga = mapper.Map<Uloga>(ulogaUpdateDto);

                var updatedUloga = ulogaRepository.UpdateUloga(mappedUloga);

                UlogaDto updatedUlogaDto = mapper.Map<UlogaDto>(updatedUloga);

                return Ok(updatedUlogaDto);
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
