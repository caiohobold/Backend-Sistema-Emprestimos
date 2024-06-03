using EmprestimosAPI.DTO.Pessoa;
using EmprestimosAPI.Helpers;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface IPessoaRepository
    {
        Task<PagedList<PessoaReadDTO>> GetAllPessoasAsync(int pageNumber, int pageSize);
        Task<PessoaReadDTO> GetPessoaByIdAsync(int id);
        Task<Pessoa> AddPessoaAsync(Pessoa pessoa);
        Task UpdatePessoaAsync(PessoaReadDTO pessoa);
        Task DeletePessoaAsync(int id);
    }
}
