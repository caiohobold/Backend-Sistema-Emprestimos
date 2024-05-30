using EmprestimosAPI.DTO.Equipamento;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipamentoReadDTO>>> GetAll()
        {
            return Ok(await _equipamentoService.GetAllEquip());
        }

        [HttpGet("available")]
        public async Task<ActionResult> GetOnlyAvailable()
        {
            var equipamentos = await _equipamentoService.GetAllAvailableEquip();
            if(equipamentos == null)
            {
                return NotFound("Nenhum equipamento disponível encontrado.");
            }
            return Ok(equipamentos);
        }

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

        [HttpPost]
        public async Task<ActionResult<EquipamentoReadDTO>> Post(EquipamentoCreateDTO equipamentoDTO)
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

        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int id, EquipamentoUpdateDTO equipamentoDTO)
        {
            await _equipamentoService.UpdateEquip(id, equipamentoDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _equipamentoService.DeleteEquip(id);
            return NoContent();
        }
    }
}
