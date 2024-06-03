using EmprestimosAPI.Helpers;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface ICategoriaRepository
    {
        Task<PagedList<Categoria>> GetAllCategAsync(int pageNumber, int pageSize);
        Task<Categoria> GetCategById(int id);
        Task<Categoria> AddCateg(Categoria categoria);
        Task UpdateCateg(Categoria categoria);
        Task DeleteCateg(int id);
    }
}
