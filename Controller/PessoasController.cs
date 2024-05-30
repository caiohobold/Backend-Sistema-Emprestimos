using EmprestimosAPI.DTO.Pessoa;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaReadDTO>>> Get()
        {
            return Ok(await _pessoaService.GetAllPessoasAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaReadDTO>> GetById(int id)
        {
            var pessoa = await _pessoaService.GetPessoaById(id);
            if (pessoa == null) return NotFound();

            return Ok(pessoa);
        }

        [HttpPost]
        public async Task<ActionResult<PessoaReadDTO>> Post(PessoaCreateDTO pessoaDTO)
        {
            var pessoaReadDto = await _pessoaService.AddPessoaAsync(pessoaDTO);

            return CreatedAtAction(nameof(GetById), new { Id = pessoaReadDto.IdPessoa }, pessoaReadDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, PessoaUpdateDTO pessoaDTO)
        {
            await _pessoaService.UpdatePessoaAsync(id, pessoaDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _pessoaService.DeletePessoaAsync(id);
            return NoContent();
        }
    }
}
