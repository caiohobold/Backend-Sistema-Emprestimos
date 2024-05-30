using EmprestimosAPI.DTO.Equipamento;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface IEquipamentoRepository
    {
        Task<IEnumerable<EquipamentoReadDTO>> GetAllEquip();
        Task<IEnumerable<Equipamento>> GetAllAvailableEquip();
        Task<Equipamento> GetEquipById(int id);
        Task<Equipamento> AddEquip(Equipamento equipamento);
        Task UpdateEquip(Equipamento equipamento);
        Task DeleteEquip(int id);
    }
}
