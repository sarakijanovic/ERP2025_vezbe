using AutoMapper;
using ERP2024.Data.AparatPorudzbinaRepository;
using ERP2024.Models.DTOs.AparatPorudzbina;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP2024.Controllers
{
    [ApiController]
    [Route("api/aparatPorudzbina")]
    public class AparatPorudzbinaController : Controller
    {
        private readonly IAparatPorudzbinaRepository aparatPorudzbinaRepository;
        private readonly IMapper mapper;

        public AparatPorudzbinaController(IMapper mapper, IAparatPorudzbinaRepository aparatPorudzbinaRepository)
        {
            this.mapper = mapper;
            this.aparatPorudzbinaRepository = aparatPorudzbinaRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<AparatPorudzbinaDto>> GetAparatiPorudzbine(int page = 1, int pageSize = 10)
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
            var aparatiPorudzbine = aparatPorudzbinaRepository.GetAparatporudzbina();

            if (aparatiPorudzbine == null || aparatiPorudzbine.Count == 0)
            {
                return NoContent();
            }

            List<AparatPorudzbinaDto> aparatiPorudzbineDto = new List<AparatPorudzbinaDto>();
            foreach (var aparatPorudzbina in aparatiPorudzbine)
            {
                aparatiPorudzbineDto.Add(mapper.Map<AparatPorudzbinaDto>(aparatPorudzbina));
            }
            var totalCount = aparatiPorudzbineDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = aparatiPorudzbineDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(itemsPerPage);

        }

        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{aparatID}/{porudzbinaID}")]
        //[Authorize(Roles = "Admin, Customer")]
        public ActionResult<AparatPorudzbinaDto> GetAparatPorudzbinaByID(Guid aparatID, Guid porudzbinaID)
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
            var aparatPorudzbina = aparatPorudzbinaRepository.GetAparatPorudzbinaById(aparatID, porudzbinaID);

            if (aparatPorudzbina == null)
            {
                return NotFound();
            }

            return mapper.Map<AparatPorudzbinaDto>(aparatPorudzbina);
        }

        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{porudzbinaID}")]
        //[Authorize(Roles = "Admin, Customer")]
        public ActionResult<List<AparatPorudzbinaDto>> GetPorudzbinaDetails(Guid porudzbinaID)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }

            var aparatiPorudzbine = aparatPorudzbinaRepository.GetAparatporudzbina().Where(a => a.porudzbinaID.Equals(porudzbinaID)).ToList(); ;

            if (aparatiPorudzbine == null || aparatiPorudzbine.Count == 0)
            {
                return NoContent();
            }

            List<AparatPorudzbinaDto> aparatiPorudzbineDto = new List<AparatPorudzbinaDto>();
            foreach (var aparatPorudzbina in aparatiPorudzbine)
            {
                aparatiPorudzbineDto.Add(mapper.Map<AparatPorudzbinaDto>(aparatPorudzbina));
            }
            return Ok(aparatiPorudzbineDto);
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Admin, Customer")]
        public ActionResult<AparatPorudzbinaDto> CreateAparatPorudzbina([FromBody] AparatPorudzbinaCreationDto aparatPorudzbinaCreationDto)
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
                AparatPorudzbina mappedAparatPorudzbina = mapper.Map<AparatPorudzbina>(aparatPorudzbinaCreationDto);
                AparatPorudzbina createdAparatPorudzbina = aparatPorudzbinaRepository.CreateAparatporudzbina(mappedAparatPorudzbina);
                return mapper.Map<AparatPorudzbinaDto>(createdAparatPorudzbina);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Create Error {ex.InnerException.Message ?? ex.Message}");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{aparatID}/{porudzbinaID}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteAparatPorudzbina(Guid aparatID, Guid porudzbinaID)
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
                var aparatPorudzbina = aparatPorudzbinaRepository.GetAparatPorudzbinaById(aparatID, porudzbinaID);
                if (aparatPorudzbina == null)
                {
                    return NotFound();
                }

                aparatPorudzbinaRepository.DeleteAparatPorudzbina(aparatID, porudzbinaID);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin")]
        public ActionResult<AparatPorudzbinaDto> UpdateAparatPorudzbina(AparatPorudzbinaUpdateDto aparatPorudzbinaUpdateDto)
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
                AparatPorudzbina mappedAparatPorudzbina = mapper.Map<AparatPorudzbina>(aparatPorudzbinaUpdateDto);

                var updatedAparatPorudzbina = aparatPorudzbinaRepository.UpdateAparatPorudzbina(mappedAparatPorudzbina);

                AparatPorudzbinaDto updatedAparatPorudzbinaDto = mapper.Map<AparatPorudzbinaDto>(updatedAparatPorudzbina);

                return Ok(updatedAparatPorudzbinaDto);
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
    }
}
