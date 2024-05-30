using EmprestimosAPI.DTO.Emprestimo;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface IEmprestimoRepository
    {
        Task<IEnumerable<Emprestimo>> GetAllEmp();
        Task<Emprestimo> GetEmpById(int id);
        Task<IEnumerable<Emprestimo>> GetActiveEmp();
        Task<Emprestimo> AddEmp(Emprestimo emprestimo);
        Task UpdateEmp(Emprestimo emprestimo);
        Task DeleteEmp(int id);
    }
}
