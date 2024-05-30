using EmprestimosAPI.DTO.Categoria;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Interfaces.Services;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoriaReadDTO>> GetAllAsync()
        {
            var categorias = await _repository.GetAllCategAsync();
            return categorias.Select(c => new CategoriaReadDTO
            {
                IdCategoria = c.IdCategoria,
                NomeCategoria = c.NomeCategoria
            }).ToList();
        }

        public async Task<CategoriaReadDTO> GetByIdAsync(int id)
        {
            var categoria = await _repository.GetCategById(id);
            if (categoria == null) return null;

            return new CategoriaReadDTO
            {
                IdCategoria = categoria.IdCategoria,
                NomeCategoria = categoria.NomeCategoria
            };
        }

        public async Task<CategoriaReadDTO> CreateAsync(CategoriaCreateDTO categoriaDTO)
        {
            var categoria = new Categoria
            {
                NomeCategoria = categoriaDTO.NomeCategoria
            };

            var newCategoria = await _repository.AddCateg(categoria);

            return new CategoriaReadDTO
            {
                IdCategoria = newCategoria.IdCategoria,
                NomeCategoria = newCategoria.NomeCategoria
            };
        }

        public async Task UpdateAsync(int id, CategoriaUpdateDTO categoriaDTO)
        {
            var categoria = await _repository.GetCategById(id);
            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoria Not Found");
            }

            categoria.NomeCategoria = categoriaDTO.NomeCategoria;

            await _repository.UpdateCateg(categoria);
        }

        public async Task DeleteAsync(int id)
        {
            var categoria = await _repository.GetCategById(id);

            if(categoria == null)
            {
                throw new KeyNotFoundException("Categoria Not Found");
            }
            await _repository.DeleteCateg(id);
        }

        
    }
}
