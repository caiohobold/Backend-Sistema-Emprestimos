using EmprestimosAPI.Data;
using EmprestimosAPI.DTO.Local;
using EmprestimosAPI.Helpers;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprestimosAPI.Repositories
{
    public class LocalRepository : ILocalRepository
    {
        private readonly DbEmprestimosContext _context;

        public LocalRepository(DbEmprestimosContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocalReadDTO>> GetAllLocaisAsync(int pageNumber, int pageSize)
        {
            var query = _context.Locais
                .Select(l => new LocalReadDTO
                {
                    IdLocal = l.IdLocal,
                    NomeLocal = l.NomeLocal
                });

            var count = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedList<LocalReadDTO>(items, count, pageNumber, pageSize);
        }

        public async Task<Local> GetLocalByIdAsync(int id)
        {
            return await _context.Locais.FindAsync(id);
        }

        public async Task<Local> AddLocalAsync(Local local)
        {
            _context.Locais.Add(local);
            await _context.SaveChangesAsync();
            return local;
        }

        public async Task UpdateLocalAsync(Local local)
        {
            _context.Entry(local).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLocalAsync(int id)
        {
            var local = await _context.Locais.FindAsync(id);
            if (local == null)
            {
                throw new KeyNotFoundException("Local não encontrado.");
            }

            _context.Locais.Remove(local);
            await _context.SaveChangesAsync();
        }
    }
}
