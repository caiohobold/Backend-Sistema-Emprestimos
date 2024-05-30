using EmprestimosAPI.DTO.Associacao;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Interfaces.Services;
using EmprestimosAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmprestimosAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociacoesController : ControllerBase
    {
        private readonly IAssociacaoService _service;

        public AssociacoesController(IAssociacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssociacaoReadDTO>>> Get()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssociacaoReadDTO>> GetById(int id)
        {
            var associacao = await _service.GetByIdAsync(id);
            if(associacao  == null)
            {
                return NotFound();
            }
            return Ok(associacao);
        }

        [HttpPost]
        public async Task<ActionResult<AssociacaoReadDTO>> Post(AssociacaoCreateDTO associacaoDTO)
        {
            var associacaoReadDto = await _service.CreateAsync(associacaoDTO);

            return CreatedAtAction(nameof(GetById), new { Id = associacaoReadDto.IdAssociacao }, associacaoReadDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, AssociacaoUpdateDTO associacaoDto)
        {
            await _service.UpdateAsync(id, associacaoDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
