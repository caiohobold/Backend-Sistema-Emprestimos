using EmprestimosAPI.DTO.Categoria;

namespace EmprestimosAPI.Interfaces.ServicesInterfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaReadDTO>> GetAllAsync();
        Task<CategoriaReadDTO> GetByIdAsync(int id);
        Task<CategoriaReadDTO> CreateAsync(CategoriaCreateDTO categoriaDTO);
        Task UpdateAsync(int id, CategoriaUpdateDTO categoriaDTO);
        Task DeleteAsync(int id);
    }
}
