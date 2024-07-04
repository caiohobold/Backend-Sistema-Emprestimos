using EmprestimosAPI.DTO.Equipamento;
using EmprestimosAPI.DTO.Local;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimosAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipamentosController : ControllerBase
    {
        private readonly IEquipamentoService _equipamentoService;

        public EquipamentosController(IEquipamentoService equipamentoService)
        {
            _equipamentoService = equipamentoService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipamentoReadDTO>>> GetAll(int pageNumber, int pageSize)
        {
            var equipamentos = await _equipamentoService.GetAllEquip(pageNumber, pageSize);
            return Ok(equipamentos);
        }

        [Authorize]
        [HttpGet("available")]
        public async Task<ActionResult> GetOnlyAvailable(int pageNumber, int pageSize)
        {
            var equipamentos = await _equipamentoService.GetAllAvailableEquip(pageNumber, pageSize);
            if (equipamentos == null)
            {
                return NotFound("Nenhum equipamento disponível encontrado.");
            }
            return Ok(equipamentos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipamentoReadDTO>> GetById(int id)
        {
            var equipamento = await _equipamentoService.GetEquipById(id);
            if (equipamento == null)
            {
                return NotFound();
            }
            return Ok(equipamento);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<EquipamentoReadDTO>> Post([FromForm] EquipamentoCreateDTO equipamentoDTO)
        {
            try
            {
                var equipamentoReadDto = await _equipamentoService.AddEquip(equipamentoDTO);
                return CreatedAtAction(nameof(GetById), new { Id = equipamentoReadDto.IdEquipamento }, equipamentoReadDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Associacao")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] EquipamentoUpdateDTO equipamentoDTO)
        {
            await _equipamentoService.UpdateEquip(id, equipamentoDTO);
            return Ok();
        }

        [HttpPatch("{id}/local")]
        public async Task<ActionResult> UpdateLocal(int id, [FromBody] UpdateLocalDTO updateLocalDTO)
        {
            try
            {
                await _equipamentoService.UpdateLocal(id, updateLocalDTO);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPatch("{id}/estado-equipamento")]
        public async Task<ActionResult> UpdateEstado(int id, [FromBody] UpdateEstadoEquipamentoDTO updateEstadoDTO)
        {
            try
            {
                await _equipamentoService.UpdateEstado(id, updateEstadoDTO);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _equipamentoService.DeleteEquip(id);
            return NoContent();
        }
    }
}
