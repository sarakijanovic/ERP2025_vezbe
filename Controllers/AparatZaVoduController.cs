using AutoMapper;
using ERP2024.Data.AparatZaVoduRepository;
using ERP2024.Models.DTOs.AparatZaVodu;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Security.Claims;

namespace ERP2024.Controllers
{
    [ApiController]
    [Route("api/aparatZaVodu")]
    public class AparatZaVoduController : Controller
    {
        private readonly IAparatZaVoduRepository aparatRepository;
        private readonly IMapper mapper;

        public AparatZaVoduController(IMapper mapper, IAparatZaVoduRepository aparatRepository)
        {
            this.mapper = mapper;
            this.aparatRepository = aparatRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<AparatZaVoduDto>> GetAparati(int page = 1, int pageSize = 10, string tipAparataID = null, string proizvodjac = null, bool sortByCena = false, string sortOrder = "asc")
        {
            var aparati = aparatRepository.GetAparati();

            if (!string.IsNullOrEmpty(tipAparataID))
            {
                Guid id;
                if (Guid.TryParse(tipAparataID, out id))
                {
                    aparati = aparati.Where(a => a.tipAparataID.Equals(id)).ToList();
                }
                else
                {
                    return BadRequest("Neispravan tipAparataID.");
                }
            }

            if (!string.IsNullOrEmpty(proizvodjac))
            {
                aparati = aparati.Where(a => a.proizvodjac.Contains(proizvodjac, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (sortByCena)
            {
                aparati = sortOrder.ToLower() == "asc" ? aparati.OrderBy(a => a.cena).ToList() : aparati.OrderByDescending(a => a.cena).ToList();
                /*switch (sortBy.ToLower())
                {
                    case "proizvodjac":
                        aparati = sortOrder.ToLower() == "asc" ? aparati.OrderBy(a => a.proizvodjac).ToList() : aparati.OrderByDescending(a => a.proizvodjac).ToList();
                        break;
                    case "cena":
                        aparati = sortOrder.ToLower() == "asc" ? aparati.OrderBy(a => a.cena).ToList() : aparati.OrderByDescending(a => a.cena).ToList();
                        break;
                    case "model":
                        aparati = sortOrder.ToLower() == "asc" ? aparati.OrderBy(a => a.model).ToList() : aparati.OrderByDescending(a => a.model).ToList();
                        break;
                        // Dodajte dodatne slučajeve za sortiranje po drugim svojstvima ako je potrebno
                }*/
            }

            if (aparati == null || aparati.Count == 0)
            {
                return NoContent();
            }

            List<AparatZaVoduDto> aparatiDto = new List<AparatZaVoduDto>();
            foreach (var aparat in aparati)
            {
                aparatiDto.Add(mapper.Map<AparatZaVoduDto>(aparat));
            }

            var totalCount = aparatiDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = aparatiDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(itemsPerPage);

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{aparatID}")]
        [AllowAnonymous]
        public ActionResult<AparatZaVoduDto> GetAparatByID(Guid aparatID)
        {
            var aparat = aparatRepository.GetAparatById(aparatID);

            if (aparat == null)
            {
                return NotFound();
            }

            return mapper.Map<AparatZaVoduDto>(aparat);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AparatZaVoduDto> CreateAparat([FromBody] AparatZaVoduCreationDto aparatCreationDto)
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
                AparatZaVodu mappedAparat = mapper.Map<AparatZaVodu>(aparatCreationDto);
                mappedAparat.aparatID = Guid.NewGuid();
                AparatZaVodu createdAparat = aparatRepository.CreateAparat(mappedAparat);
                return mapper.Map<AparatZaVoduDto>(createdAparat);
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
        [HttpDelete("{aparatID}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteAparat(Guid aparatID)
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
                var aparat = aparatRepository.GetAparatById(aparatID);
                if (aparat == null)
                {
                    return NotFound();
                }

                aparatRepository.DeleteAparat(aparatID);

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AparatZaVoduDto> UpdateAparat(AparatZaVoduUpdateDto aparatUpdateDto)
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
                AparatZaVodu mappedAparat = mapper.Map<AparatZaVodu>(aparatUpdateDto);

                var updatedAparat = aparatRepository.UpdateAparat(mappedAparat);

                AparatZaVoduDto updatedAparatDto = mapper.Map<AparatZaVoduDto>(updatedAparat);

                return Ok(updatedAparatDto);
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

        [HttpGet("/naStanju")]
        //[Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<AparatZaVoduDto>> GetAparatiNaStanju(int page = 1, int pageSize = 10)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin" || c.Value == "User"));

            if (roleClaim == null)
            {
                return Forbid();
            }

            var aparati = aparatRepository.GetAparati().Where(a => a.kolicinaNaStanju > 0).ToList();

            if (aparati == null || aparati.Count == 0)
            {
                return NoContent();
            }

            List<AparatZaVoduDto> aparatiDto = new List<AparatZaVoduDto>();
            foreach (var aparat in aparati)
            {
                aparatiDto.Add(mapper.Map<AparatZaVoduDto>(aparat));
            }

            var totalCount = aparatiDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = aparatiDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(itemsPerPage);

        }
    }
}
