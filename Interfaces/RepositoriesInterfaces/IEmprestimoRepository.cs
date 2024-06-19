using EmprestimosAPI.DTO.Emprestimo;
using EmprestimosAPI.Helpers;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface IEmprestimoRepository
    {
        Task<PagedList<Emprestimo>> GetAllEmp(int pageNumber, int pageSize);
        Task<Emprestimo> GetEmpById(int id);
        Task<IEnumerable<Emprestimo>> GetEmpByPessoaId(int idPessoa);
        Task<PagedList<Emprestimo>> GetActiveEmp(int pageNumber, int pageSize);
        Task<IEnumerable<Emprestimo>> GetEmpAtrasados();
        Task<Emprestimo> AddEmp(Emprestimo emprestimo);
        Task UpdateEmp(Emprestimo emprestimo);
        Task DeleteEmp(int id);
    }
}
