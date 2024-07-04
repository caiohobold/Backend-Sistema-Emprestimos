using EmprestimosAPI.DTO.Emprestimo;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimosAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimosController : ControllerBase
    {
        private readonly IEmprestimoService _emprestimoService;

        public EmprestimosController(IEmprestimoService emprestimoService)
        {
            _emprestimoService = emprestimoService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmprestimoReadDTO>>> GetAll(int pageNumber, int pageSize)
        {
            var emprestimos = await _emprestimoService.GetAllEmp(pageNumber, pageSize);
            return Ok(emprestimos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<EmprestimoReadDTO>> GetById(int id)
        {
            var emprestimo = await _emprestimoService.GetEmpById(id);
            if(emprestimo == null)
            {
                return NotFound();
            }
            return Ok(emprestimo);
        }

        [Authorize]
        [HttpGet("pessoa/{idPessoa}")]
        public async Task<ActionResult<IEnumerable<EmprestimoReadDTO>>> GetEmpByPessoaId(int idPessoa)
        {
            var emprestimos = await _emprestimoService.GetEmpByPessoaId(idPessoa);
            if(emprestimos == null || !emprestimos.Any())
            {
                return NotFound("Nenhum empréstimo encontrado para essa pessoa");
            }
            return Ok(emprestimos);
        }

        [Authorize]
        [HttpGet("active")]
        public async Task<ActionResult> GetOnlyActive(int pageNumber, int pageSize)
        {
            var emprestimos = await _emprestimoService.GetActiveEmp(pageNumber, pageSize);
            if(emprestimos == null || !emprestimos.Any())
            {
                return NotFound("Nenhum empréstimo ativo encontrado.");
            }
            return Ok(emprestimos);
        }

        [Authorize]
        [HttpGet("atrasados")]
        public async Task<ActionResult<IEnumerable<EmprestimoReadDTO>>> GetEmprestimosAtrasados()
        {
            var emprestimos = await _emprestimoService.GetEmpAtrasados();
            return Ok(emprestimos);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<EmprestimoReadDTO>> Post(EmprestimoCreateDTO emprestimoDTO)
        {
            var emprestimoReadDto = await _emprestimoService.AddEmp(emprestimoDTO);

            return CreatedAtAction(nameof(GetById), new { Id = emprestimoReadDto.Id }, emprestimoReadDto);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, EmprestimoUpdateDTO emprestimoDTO)
        {
            await _emprestimoService.UpdateEmp(id, emprestimoDTO);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _emprestimoService.DeleteEmp(id);
            return NoContent();
        }

        [Authorize]
        [HttpPost("{id}/finalizar")]
        public async Task<ActionResult> FinalizarEmprestimo(int id)
        {
            try
            {
                await _emprestimoService.FinalizarEmp(id);
                return Ok("Empréstimo finalizado com sucesso.");
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (InvalidOperationException ioe)
            {
                return BadRequest(ioe.Message);
            }
        }
    }
}
