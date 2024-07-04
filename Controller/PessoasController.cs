using EmprestimosAPI.DTO.Pessoa;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimosAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;

        public PessoasController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaReadDTO>>> Get(int pageNumber, int pageSize)
        {
            var pessoas = await _pessoaService.GetAllPessoasAsync(pageNumber, pageSize);
            return Ok(pessoas);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaReadDTO>> GetById(int id)
        {
            var pessoa = await _pessoaService.GetPessoaById(id);
            if (pessoa == null) return NotFound();

            return Ok(pessoa);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PessoaReadDTO>> Post(PessoaCreateDTO pessoaDTO)
        {
            var pessoaReadDto = await _pessoaService.AddPessoaAsync(pessoaDTO);

            return CreatedAtAction(nameof(GetById), new { Id = pessoaReadDto.IdPessoa }, pessoaReadDto);
        }

        [Authorize(Roles = "Associacao, Usuario")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, PessoaUpdateDTO pessoaDTO)
        {
            await _pessoaService.UpdatePessoaAsync(id, pessoaDTO);
            return Ok();
        }

        [Authorize(Roles = "Associacao, Usuario")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _pessoaService.DeletePessoaAsync(id);
            return NoContent();
        }
    }
}
