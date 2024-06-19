using EmprestimosAPI.DTO.Local;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface ILocalRepository
    {
        Task<IEnumerable<LocalReadDTO>> GetAllLocaisAsync(int pageNumber, int pageSize);
        Task<Local> GetLocalByIdAsync(int id);
        Task<Local> AddLocalAsync(Local local);
        Task UpdateLocalAsync(Local local);
        Task DeleteLocalAsync(int id);
    }
}
