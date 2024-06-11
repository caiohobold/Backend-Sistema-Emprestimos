using EmprestimosAPI.Data;
using EmprestimosAPI.DTO.Emprestimo;
using EmprestimosAPI.Helpers;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.Diagnostics.CodeAnalysis;

namespace EmprestimosAPI.Repositories
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        private readonly DbEmprestimosContext _context;
        
        public EmprestimoRepository(DbEmprestimosContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Emprestimo>> GetAllEmp(int pageNumber, int pageSize)
        {
            var query = _context.Emprestimos
                .Include(e => e.Pessoa)
                .Include(e => e.Usuario)
                .Include(e => e.Equipamento);

            var count = await query.CountAsync();

            var items = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<Emprestimo>(items, count, pageNumber, pageSize);

        }

        public async Task<Emprestimo> GetEmpById(int id)
        {
            var emprestimo = await _context.Emprestimos
                                            .Include(e => e.Pessoa)
                                            .Include(e => e.Usuario)
                                            .Include(e => e.Equipamento)
                                            .FirstOrDefaultAsync(e => e.Id == id);

            return emprestimo;
        }

        public async Task<IEnumerable<Emprestimo>> GetEmpByPessoaId(int idPessoa)
        {
            return await _context.Emprestimos
                .Include(e => e.Pessoa)
                .Include(e => e.Equipamento)
                .Where(e => e.IdPessoa == idPessoa)
                .ToListAsync();
        }

        public async Task<PagedList<Emprestimo>> GetActiveEmp(int pageNumber, int pageSize)
        {
            var query = _context.Emprestimos
                .Include(e => e.Pessoa)
                .Include(e => e.Usuario)
                .Include(e => e.Equipamento)
                .Where(e => e.Status == 0);

            var count = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<Emprestimo>(items, count, pageNumber, pageSize);
        }

        public async Task<Emprestimo> AddEmp(Emprestimo emprestimo)
        {

            var emprestimoAtivo = await _context.Emprestimos
                .AnyAsync(e => e.IdEquipamento == emprestimo.IdEquipamento && e.Status == 0);

            if (emprestimoAtivo)
            {
                throw new InvalidOperationException("Este equipamento já está emprestado.");
            }
            _context.Entry(emprestimo).State = EntityState.Modified;
            _context.Emprestimos.Add(emprestimo);
            await _context.SaveChangesAsync();
            return emprestimo;
        }

        public async Task UpdateEmp(Emprestimo emprestimo)
        {
            emprestimo.DataEmprestimo = DateTime.SpecifyKind(emprestimo.DataEmprestimo, DateTimeKind.Utc);
            emprestimo.DataDevolucaoEmprestimo = DateTime.SpecifyKind(emprestimo.DataDevolucaoEmprestimo, DateTimeKind.Utc);

            _context.Entry(emprestimo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmp(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);
            _context.Emprestimos.Remove(emprestimo);
            await _context.SaveChangesAsync();
        }
    }
}
