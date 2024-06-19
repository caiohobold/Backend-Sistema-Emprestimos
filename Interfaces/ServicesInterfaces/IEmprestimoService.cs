using EmprestimosAPI.DTO.Emprestimo;
using EmprestimosAPI.DTO.Equipamento;

namespace EmprestimosAPI.Interfaces.ServicesInterfaces
{
    public interface IEmprestimoService
    {
        Task<IEnumerable<EmprestimoReadDTO>> GetAllEmp(int pageNumber, int pageSize);
        Task<EmprestimoReadDTO> GetEmpById(int id);
        Task<IEnumerable<EmprestimoReadDTO>> GetEmpByPessoaId(int idPessoa);
        Task<IEnumerable<EmprestimoReadDTO>> GetActiveEmp(int pageNumber, int pageSize);
        Task<IEnumerable<EmprestimoReadDTO>> GetEmpAtrasados(); 
        Task<EmprestimoReadDTO> AddEmp(EmprestimoCreateDTO emprestimoDTO);
        Task UpdateEmp(int id, EmprestimoUpdateDTO emprestimoDTO);
        Task DeleteEmp(int id);
        Task FinalizarEmp(int id);
    }
}
