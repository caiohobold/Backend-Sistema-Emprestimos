using EmprestimosAPI.Data;
using EmprestimosAPI.Helpers;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EmprestimosAPI.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly DbEmprestimosContext _context;

        public CategoriaRepository(DbEmprestimosContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Categoria>> GetAllCategAsync(int pageNumber, int pageSize)
        {
            var query = _context.Categorias.AsQueryable();
            return await PagedList<Categoria>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Categoria> GetCategById(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        public async Task<Categoria> AddCateg(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task UpdateCateg(Categoria categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCateg(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if(categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }
    }
}
