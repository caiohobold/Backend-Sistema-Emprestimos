using EmprestimosAPI.DTO.Emprestimo;
using EmprestimosAPI.DTO.Equipamento;

namespace EmprestimosAPI.Interfaces.ServicesInterfaces
{
    public interface IEmprestimoService
    {
        Task<IEnumerable<EmprestimoReadDTO>> GetAllEmp();
        Task<EmprestimoReadDTO> GetEmpById(int id);
        Task<IEnumerable<EmprestimoReadDTO>> GetActiveEmp();
        Task<EmprestimoReadDTO> AddEmp(EmprestimoCreateDTO emprestimoDTO);
        Task UpdateEmp(int id, EmprestimoUpdateDTO emprestimoDTO);
        Task DeleteEmp(int id);
        Task FinalizarEmp(int id);
    }
}
