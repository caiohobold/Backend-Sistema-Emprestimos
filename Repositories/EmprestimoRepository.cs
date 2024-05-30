using EmprestimosAPI.Data;
using EmprestimosAPI.DTO.Emprestimo;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace EmprestimosAPI.Repositories
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        private readonly DbEmprestimosContext _context;
        
        public EmprestimoRepository(DbEmprestimosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Emprestimo>> GetAllEmp()
        {
            return await _context.Emprestimos
                .Include(e => e.Pessoa)
                .Include(e => e.Usuario)
                .Include(e => e.Equipamento)
                .ToListAsync();
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

        public async Task<IEnumerable<Emprestimo>> GetActiveEmp()
        {
            return await _context.Emprestimos
                .Include(e => e.Pessoa)
                .Include(e => e.Usuario)
                .Include(e => e.Equipamento)
                .Where(e => e.Status == 0)
                .ToListAsync();
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
            emprestimo.DataEmprestimo = DateTime.SpecifyKind(emprestimo.DataEmprestimo, DateTimeKind.Unspecified);

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
