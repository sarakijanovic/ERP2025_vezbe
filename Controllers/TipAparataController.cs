using AutoMapper;
using ERP2024.Data.TipAparataRepository;
using ERP2024.Models.DTOs.TipAparata;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP2024.Controllers
{
    [ApiController]
    [Route("api/tipAparata")]
    public class TipAparataController : Controller
    {
        private readonly ITipAparataRepository tipAparataRepository;
        private readonly IMapper mapper;

        public TipAparataController(IMapper mapper, ITipAparataRepository tipAparataRepository)
        {
            this.mapper = mapper;
            this.tipAparataRepository = tipAparataRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<TipAparataDto>> GetTipoviAparata(int page = 1, int pageSize = 10)
        {
            var tipoviAparata = tipAparataRepository.GetTipAparata();
            
            if (tipoviAparata == null || tipoviAparata.Count == 0)
            {
                return NoContent();
            }

            List<TipAparataDto> tipoviAparataDto = new List<TipAparataDto>();
            foreach (var tip in tipoviAparata)
            {
                tipoviAparataDto.Add(mapper.Map<TipAparataDto>(tip));
            }

            var totalCount = tipoviAparataDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = tipoviAparataDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{tipAparataID}")]
        [AllowAnonymous]
        public ActionResult<TipAparataDto> GetTipAparataByID(Guid tipAparataID)
        {
            var tipAparata = tipAparataRepository.GetTipAparataById(tipAparataID);

            if (tipAparata == null)
            {
                return NotFound();
            }

            return mapper.Map<TipAparataDto>(tipAparata);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TipAparataDto> CreateTipAparata([FromBody] TipAparataCreationDto tipAparataCreationDto)
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
                TipAparata mappedTipAparata = mapper.Map<TipAparata>(tipAparataCreationDto);
                mappedTipAparata.tipAparataID = Guid.NewGuid();
                TipAparata createdTipAparata = tipAparataRepository.CreateTipAparata(mappedTipAparata);
                return mapper.Map<TipAparataDto>(createdTipAparata);
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
        [HttpDelete("{tipAparataID}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteTipAparata(Guid tipAparataID)
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
                var tipAparata = tipAparataRepository.GetTipAparataById(tipAparataID);
                if (tipAparata == null)
                {
                    return NotFound();
                }

                tipAparataRepository.DeleteTipAparata(tipAparataID);

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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TipAparataDto> UpdateTipAparata(TipAparataUpdateDto tipAparataUpdateDto)
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
                TipAparata mappedTipAparata = mapper.Map<TipAparata>(tipAparataUpdateDto);

                var updatedTipAparata = tipAparataRepository.UpdateTipAparata(mappedTipAparata);

                TipAparataDto updatedTipAparataDto = mapper.Map<TipAparataDto>(updatedTipAparata);

                return Ok(updatedTipAparataDto);
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
