using EmprestimosAPI.DTO.Equipamento;
using EmprestimosAPI.Helpers;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface IEquipamentoRepository
    {
        Task<PagedList<EquipamentoReadDTO>> GetAllEquip(int pageNumber, int pageSize);
        Task<PagedList<Equipamento>> GetAllAvailableEquip(int pageNumber, int pageSize);
        Task<Equipamento> GetEquipById(int id);
        Task<Equipamento> AddEquip(Equipamento equipamento);
        Task UpdateEquip(Equipamento equipamento);
        Task DeleteEquip(int id);
    }
}
