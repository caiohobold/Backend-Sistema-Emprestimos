using EmprestimosAPI.DTO.Categoria;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimosAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaReadDTO>>> Get(int pageNumber, int pageSize)
        {
            var categorias = await _categoriaService.GetAllAsync(pageNumber, pageSize);
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaReadDTO>> GetById(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if (categoria == null) return NotFound();

            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaReadDTO>> Post(CategoriaCreateDTO categoriaDTO)
        {
            var categoriaReadDto = await _categoriaService.CreateAsync(categoriaDTO);

            return CreatedAtAction(nameof(GetById), new { Id = categoriaReadDto.IdCategoria }, categoriaReadDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CategoriaUpdateDTO categoriaDTO)
        {
            await _categoriaService.UpdateAsync(id, categoriaDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _categoriaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
