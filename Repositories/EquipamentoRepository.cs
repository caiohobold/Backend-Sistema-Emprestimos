using EmprestimosAPI.Data;
using EmprestimosAPI.DTO.Equipamento;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Runtime.InteropServices;

namespace EmprestimosAPI.Repositories
{
    public class EquipamentoRepository : IEquipamentoRepository
    {
        private readonly DbEmprestimosContext _context;

        public EquipamentoRepository(DbEmprestimosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EquipamentoReadDTO>> GetAllEquip()
        {
            var equipamentos = await _context.Equipamentos
                .Select(e => new EquipamentoReadDTO
                {
                    IdEquipamento = e.IdEquipamento,
                    NomeEquipamento = e.NomeEquipamento,
                    NomeCategoriaEquipamento = e.Categoria.NomeCategoria,
                    EstadoEquipamento = e.EstadoEquipamento,
                    CargaEquipamento = e.CargaEquipamento,
                    DescricaoEquipamento = e.DescricaoEquipamento,
                    StatusEquipamento = _context.Emprestimos
                                        .Where(emp => emp.IdEquipamento == e.IdEquipamento)
                                        .OrderByDescending(emp => emp.Status == 0)
                                        .ThenByDescending(emp => emp.DataEmprestimo)
                                        .Select(emp => (int?)emp.Status)
                                        .FirstOrDefault() ?? -1
                }).ToListAsync();

            return equipamentos;
            //return await _context.Equipamentos.Include(e => e.Categoria).ToListAsync();
        } 

        public async Task<IEnumerable<Equipamento>> GetAllAvailableEquip()
        {
            return await _context.Equipamentos
                .Where(e => !_context.Emprestimos.Any(emp => emp.IdEquipamento == e.IdEquipamento && emp.Status == 0))
                .Include(e => e.Categoria)
                .ToListAsync();
        }

        public async Task<Equipamento> GetEquipById(int id)
        {
            var equipamento = await _context.Equipamentos
                                .Include(e => e.Categoria)
                                .FirstOrDefaultAsync(e => e.IdEquipamento == id);

            return equipamento;
        }

        public async Task<Equipamento> AddEquip(Equipamento equipamento)
        {
            _context.Equipamentos.Add(equipamento);
            await _context.SaveChangesAsync();
            return equipamento;
        }

        public async Task UpdateEquip(Equipamento equipamento)
        {
            _context.Entry(equipamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEquip(int id)
        {
            var equipamento = await _context.Equipamentos.FindAsync(id);

            if(equipamento == null)
            {
                throw new KeyNotFoundException("Equipamento não encontrado.");
            }

            var emprestimosAtivos = await _context.Emprestimos
                                        .Where(e => e.IdEquipamento == id && e.Status == 0)
                                        .AnyAsync();

            if (emprestimosAtivos)
            {
                throw new InvalidOperationException("Não é possível remover um equipamento que está atualmente emprestado.");
            }
            _context.Equipamentos.Remove(equipamento);
            await _context.SaveChangesAsync();
        }
    }
}
