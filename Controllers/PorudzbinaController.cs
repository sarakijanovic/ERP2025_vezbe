using AutoMapper;
using ERP2024.Data.PorudzbinaRepository;
using ERP2024.Data.UlogaRepository;
using ERP2024.Helpers;
using ERP2024.Models.DTOs.Porudzbina;
using ERP2024.Models.DTOs.Uloga;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Security.Claims;

namespace ERP2024.Controllers
{
    [ApiController]
    [Route("api/porudzbina")]
    public class PorudzbinaController : Controller
    {
        private readonly IPorudzbinaRepository porudzbinaRepository;
        private readonly IMapper mapper;

        public PorudzbinaController(IMapper mapper, IPorudzbinaRepository porudzbinaRepository)
        {
            this.mapper = mapper;
            this.porudzbinaRepository = porudzbinaRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<PorudzbinaDto>> GetPorudzbine(int page = 1, int pageSize = 10, bool? dostavljena = null, string datumPorudzbine = null, bool sortByIznos = false, string sortOrder = "asc")
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            /*var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin"));

            if (roleClaim == null)
            {
                return new ObjectResult("Nemate pravo pristupa ovoj akciji!")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }*/

            var porudzbine = porudzbinaRepository.GetPorudzbine();

            if (dostavljena.HasValue)
            {
                porudzbine = porudzbine.Where(a => a.dostavljena.Equals(dostavljena)).ToList();
            }

            if (!datumPorudzbine.IsNullOrEmpty())
            {
                DateOnly datum = DateOnly.ParseExact(datumPorudzbine, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                porudzbine = porudzbine.Where(a => a.datumPorudzbine.Equals(datum)).ToList() ;
            }

            if (sortByIznos)
            {
                porudzbine = sortOrder.ToLower() == "asc" ? porudzbine.OrderBy(a => a.iznos).ToList() : porudzbine.OrderByDescending(a => a.iznos).ToList();
            }

            if (porudzbine == null || porudzbine.Count == 0)
            {
                return NoContent();
            }

            List<PorudzbinaDto> porudzbineDto = new List<PorudzbinaDto>();
            foreach (var porudzbina in porudzbine)
            {
                porudzbineDto.Add(mapper.Map<PorudzbinaDto>(porudzbina));
            }

            var totalCount = porudzbineDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = porudzbineDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("{porudzbinaID}")]
        //[Authorize(Roles = "Admin, Customer")]
        public ActionResult<PorudzbinaDto> GetPorudzbinaByID(Guid porudzbinaID)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin" || c.Value == "Customer"));

            if (roleClaim == null)
            {
                return Forbid();
            }

            var porudzbina = porudzbinaRepository.GetPorudzbinaById(porudzbinaID);

            if (porudzbina == null)
            {
                return NotFound();
            }

            return mapper.Map<PorudzbinaDto>(porudzbina);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin, Customer")]
        public ActionResult<PorudzbinaDto> CreatePorudzbina([FromBody] PorudzbinaCreationDto porudzbinaCreationDto)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin" || c.Value == "Customer"));

                if (roleClaim == null)
                {
                    return Forbid();
                }
                Porudzbina mappedPorudzbina = mapper.Map<Porudzbina>(porudzbinaCreationDto);
                mappedPorudzbina.porudzbinaID = Guid.NewGuid();
                Porudzbina createdPorudzbina = porudzbinaRepository.CreatePorudzbina(mappedPorudzbina);
                return mapper.Map<PorudzbinaDto>(createdPorudzbina);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Create Error {ex.InnerException.Message ?? ex.Message}");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{porudzbinaID}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeletePorudzbina(Guid porudzbinaID)
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

                var porudzbina = porudzbinaRepository.GetPorudzbinaById(porudzbinaID);
                if (porudzbina == null)
                {
                    return NotFound();
                }
                porudzbinaRepository.DeletePorudzbina(porudzbinaID);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [HttpPut]
        [Consumes("application/json")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PorudzbinaDto> UpdatePorudzbina(PorudzbinaUpdateDto porudzbinaUpdateDto)
        {
            try
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

                Porudzbina mappedPorudzbina = mapper.Map<Porudzbina>(porudzbinaUpdateDto);
                var updatedPorudzbina = porudzbinaRepository.UpdatePorudzbina(mappedPorudzbina);
                PorudzbinaDto updatedPorudzbinaDto = mapper.Map<PorudzbinaDto>(updatedPorudzbina);

                return Ok(updatedPorudzbinaDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Update Error {ex.InnerException.InnerException.Message ?? ex.Message}");
            }
        }


        [HttpGet("klijent/{klijentID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[Authorize(Roles = "Admin, Customer")]
        public ActionResult<List<PorudzbinaDto>> GetPorudzbineByKlijentID(string klijentID, int page = 1, int pageSize = 10)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin" || c.Value == "Customer"));

            if (roleClaim == null)
            {
                return Forbid();
            }

            Guid id = Guid.Parse(klijentID);
            var porudzbine = porudzbinaRepository.GetPorudzbine().Where(p => p.klijentID.Equals(id)).ToList();

            if (porudzbine == null || porudzbine.Count == 0)
            {
                return NoContent();
            }

            List<PorudzbinaDto> porudzbineDto = new List<PorudzbinaDto>();
            foreach (var porudzbina in porudzbine)
            {
                porudzbineDto.Add(mapper.Map<PorudzbinaDto>(porudzbina));
            }

            var totalCount = porudzbineDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = porudzbineDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);

        }

        [HttpGet("zaposleni/{zaposleniID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[Authorize(Roles = "Admin")]
        public ActionResult<List<PorudzbinaDto>> GetPorudzbineByZaposleniID(string zaposleniID, int page = 1, int pageSize = 10)
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

            Guid id = Guid.Parse(zaposleniID);
            var porudzbine = porudzbinaRepository.GetPorudzbine().Where(p => p.zaposleniID.Equals(id)).ToList();

            if (porudzbine == null || porudzbine.Count == 0)
            {
                return NoContent();
            }

            List<PorudzbinaDto> porudzbineDto = new List<PorudzbinaDto>();
            foreach (var porudzbina in porudzbine)
            {
                porudzbineDto.Add(mapper.Map<PorudzbinaDto>(porudzbina));
            }

            var totalCount = porudzbineDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = porudzbineDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);

        }
    }
}
