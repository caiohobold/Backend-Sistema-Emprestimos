﻿using EmprestimosAPI.DTO.Equipamento;
using EmprestimosAPI.DTO.Local;
using EmprestimosAPI.DTO.Usuario;

namespace EmprestimosAPI.Interfaces.ServicesInterfaces
{
    public interface IEquipamentoService
    {
        Task<IEnumerable<EquipamentoReadDTO>> GetAllEquip(int pageNumber, int pageSize);
        Task<IEnumerable<EquipamentoReadDTO>> GetAllAvailableEquip(int pageNumber, int pageSize);
        Task<EquipamentoReadDTO> GetEquipById(int id);
        Task<EquipamentoReadDTO> AddEquip(EquipamentoCreateDTO usuarioDTO);
        Task UpdateEquip(int id, EquipamentoUpdateDTO usuarioDTO);
        Task UpdateLocal(int id, UpdateLocalDTO updateLocalDTO);
        Task DeleteEquip(int id);
    }
}
