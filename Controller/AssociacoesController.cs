using EmprestimosAPI.DTO.Associacao;
using EmprestimosAPI.Interfaces.Account;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Interfaces.Services;
using EmprestimosAPI.Models;
using EmprestimosAPI.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EmprestimosAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociacoesController : ControllerBase
    {
        private readonly IAssociacaoService _service;
        private readonly IAuthenticate _authenticateService;

        public AssociacoesController(IAssociacaoService service, IAuthenticate authenticateService)
        {
            _service = service;
            _authenticateService = authenticateService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssociacaoReadDTO>>> Get(int pageNumber, int pageSize)
        {
            var associacoes = await _service.GetAllAsync(pageNumber, pageSize);
            return Ok(associacoes);
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

        [HttpPost("register")]
        public async Task<ActionResult<UserToken>> Post(AssociacaoCreateDTO associacaoDTO)
        {
            if(associacaoDTO == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var emailExiste = await _authenticateService.AssocExists(associacaoDTO.EmailProfissional);

            if (emailExiste)
            {
                return BadRequest("Este e-mail já possui um cadastro.");
            }

            var associacaoReadDto = await _service.CreateAsync(associacaoDTO);
            if (associacaoReadDto == null)
            {
                return BadRequest("Ocorreu um erro ao cadastrar.");
            }

            var token = _authenticateService.GenerateToken(associacaoReadDto.IdAssociacao, associacaoReadDto.EmailProfissional, associacaoReadDto.NomeFantasia, associacaoReadDto.NomeFantasia);


            return new UserToken
            {
                Token = token
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> LoginAssociacao(LoginModel loginModel)
        {
            var existe = await _authenticateService.AssocExists(loginModel.Email);
            if (!existe)
            {
                return Unauthorized("Associação não existe.");
            }

            var result = await _authenticateService.AuthenticateAssocAsync(loginModel.Email, loginModel.Senha);
            if (!result)
            {
                return Unauthorized("E-mail ou senha inválidos!");
            }

            var associacao = await _authenticateService.GetAssocByEmail(loginModel.Email);
            var token = _authenticateService.GenerateToken(associacao.IdAssociacao, associacao.EmailProfissional, associacao.NomeFantasia, "Associacao");

            return new UserToken { Token = token };
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

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<AssociacaoReadDTO>> GetMyAssoc()
        {
            var assocIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if(string.IsNullOrEmpty(assocIdClaim) || int.TryParse(assocIdClaim, out int assocId))
            {
                return BadRequest("Invalid Token");
            }

            var assoc = await _service.GetByIdAsync(assocId);
            if(assoc == null)
            {
                return NotFound("Assoc not found");
            }

            return Ok(assoc);
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<ActionResult> UpdateMyAssoc([FromBody] AssociacaoUpdateDTO associacaoDTO)
        {
            var assocId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _service.UpdateAsync(assocId, associacaoDTO);
            return NoContent();
        }
    }
}
