using EmprestimosAPI.DTO.Local;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocaisController : ControllerBase
    {
        private readonly ILocalService _localService;

        public LocaisController(ILocalService localService)
        {
            _localService = localService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocalReadDTO>>> GetAllLocais([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var locais = await _localService.GetAllLocaisAsync(pageNumber, pageSize);
            return Ok(locais);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocalReadDTO>> GetLocalById(int id)
        {
            var local = await _localService.GetLocalByIdAsync(id);
            if (local == null)
            {
                return NotFound();
            }

            return Ok(local);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<LocalReadDTO>> AddLocal(LocalCreateDTO localDTO)
        {
            var local = await _localService.AddLocalAsync(localDTO);
            return CreatedAtAction(nameof(GetLocalById), new { id = local.IdLocal }, local);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocal(int id, LocalUpdateDTO localDTO)
        {
            try
            {
                await _localService.UpdateLocalAsync(id, localDTO);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocal(int id)
        {
            try
            {
                await _localService.DeleteLocalAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
