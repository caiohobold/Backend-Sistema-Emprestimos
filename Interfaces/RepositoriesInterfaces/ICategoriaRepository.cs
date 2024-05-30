using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> GetAllCategAsync();
        Task<Categoria> GetCategById(int id);
        Task<Categoria> AddCateg(Categoria categoria);
        Task UpdateCateg(Categoria categoria);
        Task DeleteCateg(int id);
    }
}
