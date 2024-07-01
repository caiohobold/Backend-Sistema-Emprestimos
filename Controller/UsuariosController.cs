using EmprestimosAPI.DTO.Usuario;
using EmprestimosAPI.Interfaces.Account;
using EmprestimosAPI.Interfaces.Services;
using EmprestimosAPI.Models;
using EmprestimosAPI.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Claims;

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

        [Authorize(Roles = "Associacao")]
        [HttpPost("register-user")]
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

            var token = _authenticateService.GenerateToken(usuarioReadDto.IdUsuario, usuarioReadDto.EmailPessoal, usuarioReadDto.NomeCompleto, "Usuario");

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
            var token = _authenticateService.GenerateToken(usuario.IdUsuario, usuario.EmailPessoal, usuario.NomeCompleto, "Usuario");

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

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UsuarioReadDTO>> GetMe()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Invalid token");
            }

            var usuario = await _usuarioService.GetUserById(userId);
            if (usuario == null)
            {
                return NotFound("User not found");
            }

            return Ok(usuario);
        }


        [Authorize]
        [HttpPut("me")]
        public async Task<ActionResult> UpdateMe([FromBody] UsuarioUpdateDTO usuarioDTO)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _usuarioService.UpdateUser(userId, usuarioDTO);
            return NoContent();
        }

        [HttpPut("{id}/change-password")]
        public async Task<ActionResult> ChangePassword(int id, [FromBody] ChangePasswordDTO changePasswordDTO)
        {
            if(changePasswordDTO == null || string.IsNullOrWhiteSpace(changePasswordDTO.NovaSenha))
            {
                return BadRequest("Senha inválida.");
            }

            await _usuarioService.ChangeUserPassword(id, changePasswordDTO.NovaSenha);
            return NoContent();
        }
    }
}
