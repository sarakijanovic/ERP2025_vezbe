using AutoMapper;
using ERP2024.Data.KlijentRepository;
using ERP2024.Data.ZaposleniRepository;
using ERP2024.Models.DTOs.Klijent;
using ERP2024.Models.DTOs.Zaposleni;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP2024.Controllers

{
    [ApiController]
    [Route("api/klijent")]
    public class KlijentController : Controller
    {
        private readonly IKlijentRepository klijentRepository;
        private readonly IMapper mapper;

        public KlijentController(IMapper mapper, IKlijentRepository klijentRepository)
        {
            this.mapper = mapper;
            this.klijentRepository = klijentRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<KlijentDto>> GetKlijent(int page = 1, int pageSize = 10)
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
            var klijenti = klijentRepository.GetKlijent();

            if (klijenti == null || klijenti.Count == 0)
            {
                return NoContent();
            }

            List<KlijentDto> klijentiDto = new List<KlijentDto>();
            foreach (var klijent in klijenti)
            {
                klijentiDto.Add(mapper.Map<KlijentDto>(klijent));
            }

            var totalCount = klijentiDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = klijentiDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("id/{klijentID}")]
        //[Authorize(Roles = "Admin, User, Customer")]
        public ActionResult<KlijentDto> GetKlijentByID(Guid klijentID)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Admin" || c.Value == "Customer" || c.Value == "User"));

            if (roleClaim == null)
            {
                return Forbid();
            }
            var klijent = klijentRepository.GetKlijentById(klijentID);

            if (klijent == null)
            {
                return NotFound();
            }

            return mapper.Map<KlijentDto>(klijent);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("{korisnickoImeKlijenta}")]
        public ActionResult<KlijentDto> GetKlijentByUsername(string korisnickoImeKlijenta)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var klijent = klijentRepository.GetKlijentByUsername(korisnickoImeKlijenta);

            if (klijent == null)
            {
                return NotFound();
            }

            return mapper.Map<KlijentDto>(klijent);
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KlijentDto> CreateKlijent([FromBody] KlijentCreationDto klijentCreationDto)
        {
            try
            {
                Klijent createdKlijent = klijentRepository.CreateKlijent(klijentCreationDto);
                return mapper.Map<KlijentDto>(createdKlijent);
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
        [HttpDelete("{klijentID}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteKlijent(Guid klijentID)
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
                var klijent = klijentRepository.GetKlijentById(klijentID);
                if (klijent == null)
                {
                    return NotFound();
                }

                klijentRepository.DeleteKlijent(klijentID);

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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Admin, Customer")]
        public ActionResult<KlijentDto> UpdateKlijent(KlijentUpdateDto klijentUpdateDto)
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
                var updatedKlijent = klijentRepository.UpdateKlijent(klijentUpdateDto);

                KlijentDto updatedKlijentDto = mapper.Map<KlijentDto>(updatedKlijent);

                return Ok(updatedKlijentDto);
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
