using EmprestimosAPI.Data;
using EmprestimosAPI.DTO.Emprestimo;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
using EmprestimosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprestimosAPI.Services
{
    public class EmprestimoService : IEmprestimoService
    {
        private readonly IEmprestimoRepository _emprestimoRepository;
        private readonly DbEmprestimosContext _context;

        public EmprestimoService(IEmprestimoRepository emprestimoRepository, DbEmprestimosContext context)
        {
            _emprestimoRepository = emprestimoRepository;
            _context = context;
        }


        public async Task<IEnumerable<EmprestimoReadDTO>> GetAllEmp(int pageNumber, int pageSize)
        {
            var emprestimos = await _emprestimoRepository.GetAllEmp(pageNumber, pageSize);
            return emprestimos.Select(e => new EmprestimoReadDTO
            {
                Id = e.Id,
                IdPessoa = e.Pessoa.IdPessoa,
                IdEquipamento = e.Equipamento.IdEquipamento,
                DataEmprestimo = e.DataEmprestimo,
                IdUsuario = e.Usuario.IdUsuario,
                Status = e.Status
            }).ToList();
        }

        public async Task<EmprestimoReadDTO> GetEmpById(int id)
        {
            var emprestimo = await _emprestimoRepository.GetEmpById(id);
            if (emprestimo == null) return null;

            return new EmprestimoReadDTO
            {
                Id = emprestimo.Id,
                IdPessoa = emprestimo.Pessoa.IdPessoa,
                IdEquipamento = emprestimo.Equipamento.IdEquipamento,
                DataEmprestimo = emprestimo.DataEmprestimo,
                IdUsuario = emprestimo.Usuario.IdUsuario,
                Status = emprestimo.Status
            };
        }

        public async Task<IEnumerable<EmprestimoReadDTO>> GetActiveEmp(int pageNumber, int pageSize)
        {
            var emprestimos = await _emprestimoRepository.GetActiveEmp(pageNumber, pageSize);
            return emprestimos.Select(e => new EmprestimoReadDTO
            {
                Id = e.Id,
                IdPessoa = e.IdPessoa,
                IdEquipamento = e.IdEquipamento,
                DataEmprestimo = e.DataEmprestimo,
                IdUsuario = e.IdUsuario,
                Status = e.Status
            }).ToList();
        }

        public async Task<EmprestimoReadDTO> AddEmp(EmprestimoCreateDTO emprestimoDTO)
        {
            var emprestimo = new Emprestimo
            { 
                DataEmprestimo = emprestimoDTO.DataEmprestimo,
                IdPessoa = emprestimoDTO.IdPessoa,
                IdEquipamento = emprestimoDTO.IdEquipamento,
                IdUsuario = emprestimoDTO.IdUsuario
            };

            var newEmprestimo = await _emprestimoRepository.AddEmp(emprestimo);

            if(newEmprestimo.Pessoa == null)
            {
                await _context.Entry(newEmprestimo).Reference(e => e.Pessoa).LoadAsync();
            }
            if (newEmprestimo.Usuario == null)
            {
                await _context.Entry(newEmprestimo).Reference(e => e.Usuario).LoadAsync();
            }
            if (newEmprestimo.Equipamento == null)
            {
                await _context.Entry(newEmprestimo).Reference(e => e.Equipamento).LoadAsync();
            }

            return new EmprestimoReadDTO
            {
                Id = emprestimo.Id,
                IdPessoa = emprestimo.Pessoa.IdPessoa,
                IdEquipamento = emprestimo.Equipamento.IdEquipamento,
                DataEmprestimo = emprestimo.DataEmprestimo,
                IdUsuario = emprestimo.Usuario.IdUsuario,
                Status = emprestimo.Status
            };
            
        }

        public async Task UpdateEmp(int id, EmprestimoUpdateDTO emprestimoDTO)
        {
            var emprestimo = await _emprestimoRepository.GetEmpById(id);
            if (emprestimo == null)
            {
                throw new KeyNotFoundException("Empréstimo Not Found");
            }

            emprestimo.DataEmprestimo = emprestimoDTO.DataEmprestimo;

            await _emprestimoRepository.UpdateEmp(emprestimo);
        }

        public async Task DeleteEmp(int id)
        {
            var emprestimo = await _emprestimoRepository.GetEmpById(id);

            if(emprestimo == null)
            {
                throw new KeyNotFoundException("Empréstimo Not Found");
            }

            await _emprestimoRepository.DeleteEmp(id);
            
        }

        public async Task FinalizarEmp(int id)
        {
            var emprestimo = await _emprestimoRepository.GetEmpById(id);
            if (emprestimo == null)
            {
                throw new KeyNotFoundException("Empréstimo não encontrado.");
            }

            if (emprestimo.Status != 0)
            {
                throw new InvalidOperationException("Este empréstimo já foi finalizado.");
            }

            emprestimo.Status = 1;
            await _emprestimoRepository.UpdateEmp(emprestimo);
        }
    }
}