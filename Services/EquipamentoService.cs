using EmprestimosAPI.Data;
using EmprestimosAPI.DTO.Emprestimo;
using EmprestimosAPI.DTO.Equipamento;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
using EmprestimosAPI.Models;
using System.Collections;

namespace EmprestimosAPI.Services
{
    public class EquipamentoService : IEquipamentoService
    {
        private readonly IEquipamentoRepository _equipamentoRepository;
        private readonly DbEmprestimosContext _context;

        public EquipamentoService(IEquipamentoRepository equipamentoRepository, DbEmprestimosContext context)
        {
            _equipamentoRepository = equipamentoRepository;
            _context = context;
        }

        public async Task<IEnumerable<EquipamentoReadDTO>> GetAllEquip(int pageNumber, int pageSize)
        {
            var equipamentos = await _equipamentoRepository.GetAllEquip(pageNumber, pageSize);
            return equipamentos.Select(e => new EquipamentoReadDTO
            {
                IdEquipamento = e.IdEquipamento,
                NomeEquipamento = e.NomeEquipamento,
                NomeCategoriaEquipamento = e.NomeCategoriaEquipamento,
                EstadoEquipamento = e.EstadoEquipamento,
                CargaEquipamento = e.CargaEquipamento,
                DescricaoEquipamento = e.DescricaoEquipamento,
                StatusEquipamento = e.StatusEquipamento
            }).ToList();
        }

        public async Task<IEnumerable<EquipamentoReadDTO>> GetAllAvailableEquip(int pageNumber, int pageSize)
        {
            var equipamentos = await _equipamentoRepository.GetAllAvailableEquip(pageNumber, pageSize);
            return equipamentos.Select(e => new EquipamentoReadDTO
            {
                IdEquipamento = e.IdEquipamento,
                NomeEquipamento = e.NomeEquipamento,
                NomeCategoriaEquipamento = e.Categoria.NomeCategoria,
                EstadoEquipamento = e.EstadoEquipamento,
                CargaEquipamento = e.CargaEquipamento,
                DescricaoEquipamento = e.DescricaoEquipamento
            }).ToList();
        }

        public async Task<EquipamentoReadDTO> GetEquipById(int id)
        {
            var equipamento = await _equipamentoRepository.GetEquipById(id);
            if (equipamento == null) return null;

            return new EquipamentoReadDTO
            {
                IdEquipamento = equipamento.IdEquipamento,
                NomeEquipamento = equipamento.NomeEquipamento,
                NomeCategoriaEquipamento = equipamento.Categoria.NomeCategoria,
                EstadoEquipamento = equipamento.EstadoEquipamento,
                CargaEquipamento = equipamento.CargaEquipamento,
                DescricaoEquipamento = equipamento.DescricaoEquipamento
            };
        }

        public async Task<EquipamentoReadDTO> AddEquip(EquipamentoCreateDTO equipamentoDTO)
        {

            var equipamento = new Equipamento
            {
                NomeEquipamento = equipamentoDTO.NomeEquipamento,
                IdCategoria = equipamentoDTO.IdCategoria,
                EstadoEquipamento = equipamentoDTO.EstadoEquipamento,
                CargaEquipamento = equipamentoDTO.CargaEquipamento,
                DescricaoEquipamento = equipamentoDTO.DescricaoEquipamento
            };

            var newEquipamento = await _equipamentoRepository.AddEquip(equipamento);

            if(newEquipamento.Categoria == null)
            {
                await _context.Entry(newEquipamento).Reference(e => e.Categoria).LoadAsync();
            }

            return new EquipamentoReadDTO
            {
                IdEquipamento = newEquipamento.IdEquipamento,
                NomeEquipamento = newEquipamento.NomeEquipamento,
                NomeCategoriaEquipamento = newEquipamento.Categoria.NomeCategoria,
                EstadoEquipamento = newEquipamento.EstadoEquipamento,
                CargaEquipamento = newEquipamento.CargaEquipamento,
                DescricaoEquipamento = newEquipamento.DescricaoEquipamento
            };
        }

        public async Task UpdateEquip(int id, EquipamentoUpdateDTO equipamentoDTO)
        {
            var equipamento = await _equipamentoRepository.GetEquipById(id);
            if(equipamento == null)
            {
                throw new KeyNotFoundException("Equipamento Not Found");
            }

            equipamento.NomeEquipamento = equipamentoDTO.NomeEquipamento;
            equipamento.EstadoEquipamento = equipamentoDTO.EstadoEquipamento;
            equipamento.CargaEquipamento = equipamentoDTO.CargaEquipamento;
            equipamento.DescricaoEquipamento = equipamentoDTO.DescricaoEquipamento;

            await _equipamentoRepository.UpdateEquip(equipamento);
        }

        public async Task DeleteEquip(int id)
        {
            var equipamento = await _equipamentoRepository.GetEquipById(id);

            if(equipamento == null)
            {
                throw new KeyNotFoundException("Equipamento Not Found");
            }

            await _equipamentoRepository.DeleteEquip(id);
        }
    }
}
