﻿using EmprestimosAPI.Data;
using EmprestimosAPI.DTO.Pessoa;
using EmprestimosAPI.Helpers;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EmprestimosAPI.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly DbEmprestimosContext _context;
        public PessoaRepository(DbEmprestimosContext context)
        {
            _context = context;
        }

        public async Task<PagedList<PessoaReadDTO>> GetAllPessoasAsync(int pageNumber, int pageSize)
        {
            var query = _context.Pessoas
                .Select(p => new PessoaReadDTO
                {
                    IdPessoa = p.IdPessoa,
                    NomeCompleto = p.NomeCompleto,
                    Cpf = p.Cpf,
                    Email = p.Email,
                    Telefone = p.Telefone,
                    Descricao = p.Descricao,
                    Endereco = p.Endereco,
                    Foto1Pessoa = p.Foto1Pessoa != null ? Convert.ToBase64String(p.Foto1Pessoa) : null,
                    Foto2Pessoa = p.Foto2Pessoa != null ? Convert.ToBase64String(p.Foto2Pessoa) : null,
                    StatusEmprestimo = _context.Emprestimos
                                       .Where(e => e.IdPessoa == p.IdPessoa)
                                       .OrderByDescending(e => e.Status == 0)
                                       .ThenByDescending(e => e.DataEmprestimo)
                                       .Select(e => (int?)e.Status)
                                       .FirstOrDefault() ?? -1,
                    DataEmprestimo = _context.Emprestimos
                                       .Where(e => e.IdPessoa == p.IdPessoa)
                                       .Select(e => (DateTime?)e.DataEmprestimo)
                                       .FirstOrDefault()

                });

            var count = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();
           
            return new PagedList<PessoaReadDTO>(items, count, pageNumber, pageSize);
        }

        public async Task<PessoaReadDTO> GetPessoaByIdAsync(int id)
        {
            var pessoa = await _context.Pessoas
                .Where(p => p.IdPessoa == id)
                .Select(p => new PessoaReadDTO
                {
                    IdPessoa = p.IdPessoa,
                    NomeCompleto = p.NomeCompleto,
                    Cpf = p.Cpf,
                    Email = p.Email,
                    Telefone = p.Telefone,
                    Descricao = p.Descricao,
                    Endereco = p.Endereco,
                    Foto1Pessoa = p.Foto1Pessoa != null ? Convert.ToBase64String(p.Foto1Pessoa) : null,
                    Foto2Pessoa = p.Foto2Pessoa != null ? Convert.ToBase64String(p.Foto2Pessoa) : null,
                    StatusEmprestimo = _context.Emprestimos
                               .Where(e => e.IdPessoa == p.IdPessoa)
                               .OrderByDescending(e => e.Status == 0)
                               .ThenByDescending(e => e.DataEmprestimo)
                               .Select(e => (int?)e.Status)
                               .FirstOrDefault() ?? -1,
                    DataEmprestimo = _context.Emprestimos
                                       .Where(e => e.IdPessoa == p.IdPessoa)
                                       .Select(e => (DateTime?)e.DataEmprestimo)
                                       .FirstOrDefault()
                }).SingleOrDefaultAsync();

            return pessoa;
        }

        public async Task<Pessoa> AddPessoaAsync(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();
            return pessoa;
        }

        public async Task UpdatePessoaAsync(Pessoa pessoa)
        {
            _context.Entry(pessoa).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePessoaAsync(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if(pessoa == null)
            {
                throw new KeyNotFoundException("Pessoa não encontrada.");
            }
            var emprestimosAtivos = await _context.Emprestimos
               .Where(e => e.IdPessoa == id && e.Status == 0)
               .AnyAsync();

            if (emprestimosAtivos)
            {
                throw new InvalidOperationException("Não é possível remover uma pessoa que possui empréstimos em andamento.");
            }

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
            
        }
    }
}
