using EmprestimosAPI.DTO.Equipamento;
using EmprestimosAPI.DTO.Usuario;

namespace EmprestimosAPI.Interfaces.ServicesInterfaces
{
    public interface IEquipamentoService
    {
        Task<IEnumerable<EquipamentoReadDTO>> GetAllEquip();
        Task<IEnumerable<EquipamentoReadDTO>> GetAllAvailableEquip();
        Task<EquipamentoReadDTO> GetEquipById(int id);
        Task<EquipamentoReadDTO> AddEquip(EquipamentoCreateDTO usuarioDTO);
        Task UpdateEquip(int id, EquipamentoUpdateDTO usuarioDTO);
        Task DeleteEquip(int id);
    }
}
