using EmprestimosAPI.DTO.Usuario;
using EmprestimosAPI.Interfaces.Account;
using EmprestimosAPI.Interfaces.Services;
using EmprestimosAPI.Models;
using EmprestimosAPI.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace EmprestimosAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthenticate _authenticateService;

        public UsuariosController(IUsuarioService usuarioService, IAuthenticate authenticateService)
        {
            _usuarioService = usuarioService;
            _authenticateService = authenticateService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioReadDTO>>> GetAll(int pageNumber, int pageSize)
        {
            var users = await _usuarioService.GetAllUsers(pageNumber, pageSize);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioReadDTO>> GetById(int id)
        {
            var usuario = await _usuarioService.GetUserById(id);
            if(usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserToken>> Post(UsuarioCreateDTO usuarioDTO)
        {
            if(usuarioDTO == null)
            {
                return BadRequest("Dados inválidos");
            }
            
            var emailExiste = await _authenticateService.UserExists(usuarioDTO.EmailPessoal);

            if(emailExiste)
            {
                return BadRequest("Este e-mail já possui um cadastro.");
            }


            var usuarioReadDto = await _usuarioService.AddUser(usuarioDTO);
            if(usuarioReadDto == null)
            {
                return BadRequest("Ocorreu um erro ao cadastrar.");
            }

            var token = _authenticateService.GenerateToken(usuarioReadDto.IdUsuario, usuarioReadDto.EmailPessoal, usuarioReadDto.NomeCompleto, usuarioReadDto.AssociacaoNomeFantasia);

            return new UserToken
            {
                Token = token
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserToken>> Selecionar(LoginModel loginModel)
        {
            var existe = await _authenticateService.UserExists(loginModel.Email);
            if (!existe)
            {
                return Unauthorized("Usuário não existe.");
            }

            var result = await _authenticateService.AuthenticateAsync(loginModel.Email, loginModel.Senha);
            if(!result)
            {
                return Unauthorized("E-mail ou senha inválidos!");
            }

            var usuario = await _authenticateService.GetUserByEmail(loginModel.Email);
            var token = _authenticateService.GenerateToken(usuario.IdUsuario, usuario.EmailPessoal, usuario.NomeCompleto, usuario.Associacao.NomeFantasia);

            return new UserToken { Token = token };
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int id, UsuarioUpdateDTO usuarioDTO)
        {
            await _usuarioService.UpdateUser(id, usuarioDTO);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _usuarioService.DeleteUser(id);
            return NoContent();
        }
    }
}
