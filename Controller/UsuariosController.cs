using EmprestimosAPI.DTO.Usuario;
using EmprestimosAPI.Interfaces.Services;
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

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioReadDTO>>> GetAll()
        {
            return Ok(await _usuarioService.GetAllUsers());
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

        [HttpPost]
        public async Task<ActionResult<UsuarioReadDTO>> Post(UsuarioCreateDTO usuarioDTO)
        {
            var usuarioReadDto = await _usuarioService.AddUser(usuarioDTO);

            return CreatedAtAction(nameof(GetById), new { Id = usuarioReadDto.IdUsuario }, usuarioReadDto);
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
