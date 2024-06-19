﻿using EmprestimosAPI.Data;
using EmprestimosAPI.DTO.Emprestimo;
using EmprestimosAPI.DTO.Equipamento;
using EmprestimosAPI.DTO.Local;
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
                Foto1 = e.Foto1,
                Foto2 = e.Foto2,
                StatusEquipamento = e.StatusEquipamento,
                IdLocal = e.IdLocal,
                NomeLocal = e.NomeLocal
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
                DescricaoEquipamento = e.DescricaoEquipamento,
                IdLocal = e.IdLocal,
                Foto1 = e.Foto1 != null ? Convert.ToBase64String(e.Foto1) : null,
                Foto2 = e.Foto2 != null ? Convert.ToBase64String(e.Foto2) : null
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
                DescricaoEquipamento = equipamento.DescricaoEquipamento,
                IdLocal = equipamento.IdLocal,
                NomeLocal = equipamento.Local.NomeLocal,
                Foto1 = equipamento.Foto1 != null ? Convert.ToBase64String(equipamento.Foto1) : null,
                Foto2 = equipamento.Foto2 != null ? Convert.ToBase64String(equipamento.Foto2) : null
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
                DescricaoEquipamento = equipamentoDTO.DescricaoEquipamento,
                IdLocal = equipamentoDTO.IdLocal
            };

            if (equipamentoDTO.Foto1 != null)
            {
                using (var stream = new MemoryStream())
                {
                    await equipamentoDTO.Foto1.CopyToAsync(stream);
                    equipamento.Foto1 = stream.ToArray();
                }
            }

            if (equipamentoDTO.Foto2 != null)
            {
                using (var stream = new MemoryStream())
                {
                    await equipamentoDTO.Foto2.CopyToAsync(stream);
                    equipamento.Foto2 = stream.ToArray();
                }
            }

            var newEquipamento = await _equipamentoRepository.AddEquip(equipamento);

            if (newEquipamento.Categoria == null)
            {
                await _context.Entry(newEquipamento).Reference(e => e.Categoria).LoadAsync();
            }

            if (newEquipamento.Local == null)
            {
                await _context.Entry(newEquipamento).Reference(e => e.Local).LoadAsync();
            }

            return new EquipamentoReadDTO
            {
                IdEquipamento = newEquipamento.IdEquipamento,
                NomeEquipamento = newEquipamento.NomeEquipamento,
                NomeCategoriaEquipamento = newEquipamento.Categoria.NomeCategoria,
                EstadoEquipamento = newEquipamento.EstadoEquipamento,
                CargaEquipamento = newEquipamento.CargaEquipamento,
                DescricaoEquipamento = newEquipamento.DescricaoEquipamento,
                Foto1 = newEquipamento.Foto1 != null ? Convert.ToBase64String(newEquipamento.Foto1) : null,
                Foto2 = newEquipamento.Foto2 != null ? Convert.ToBase64String(newEquipamento.Foto2) : null,
                IdLocal = newEquipamento.IdLocal,
                NomeLocal = newEquipamento.Local.NomeLocal
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
            equipamento.IdLocal = equipamentoDTO.IdLocal;
            
            if(equipamentoDTO.Foto1 != null)
            {
                using (var stream = new MemoryStream())
                {
                    await equipamentoDTO.Foto1.CopyToAsync(stream);
                    equipamento.Foto1 = stream.ToArray();
                }
            }

            if (equipamentoDTO.Foto2 != null)
            {
                using (var stream = new MemoryStream())
                {
                    await equipamentoDTO.Foto2.CopyToAsync(stream);
                    equipamento.Foto2 = stream.ToArray();
                }
            }

            await _equipamentoRepository.UpdateEquip(equipamento);
        }

        public async Task UpdateLocal(int id, UpdateLocalDTO updateLocalDTO)
        {
            var equipamento = await _equipamentoRepository.GetEquipById(id);
            if(equipamento == null)
            {
                throw new KeyNotFoundException("Equipamento não encontrado");
            }

            equipamento.IdLocal = updateLocalDTO.IdLocal;
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
