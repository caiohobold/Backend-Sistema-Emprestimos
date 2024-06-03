using EmprestimosAPI.DTO.Pessoa;

namespace EmprestimosAPI.Interfaces.ServicesInterfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<PessoaReadDTO>> GetAllPessoasAsync(int pageNumber, int pageSize);
        Task<PessoaReadDTO> GetPessoaById(int id);
        Task<PessoaReadDTO> AddPessoaAsync(PessoaCreateDTO pessoaDTO);
        Task UpdatePessoaAsync(int id, PessoaUpdateDTO pessoaDTO);
        Task DeletePessoaAsync(int id);
    }
}
