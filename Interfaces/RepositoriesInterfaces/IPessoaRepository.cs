using EmprestimosAPI.DTO.Pessoa;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface IPessoaRepository
    {
        Task<IEnumerable<PessoaReadDTO>> GetAllPessoasAsync();
        Task<PessoaReadDTO> GetPessoaByIdAsync(int id);
        Task<Pessoa> AddPessoaAsync(Pessoa pessoa);
        Task UpdatePessoaAsync(PessoaReadDTO pessoa);
        Task DeletePessoaAsync(int id);
    }
}
