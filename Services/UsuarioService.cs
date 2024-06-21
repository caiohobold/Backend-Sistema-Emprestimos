using EmprestimosAPI.Interfaces.Services;
using EmprestimosAPI.DTO;
using EmprestimosAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.DTO.Usuario;
using Microsoft.EntityFrameworkCore;
using EmprestimosAPI.Data;

namespace EmprestimosAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly DbEmprestimosContext _context;
        private readonly HashingService _hashingService;

        public UsuarioService(IUsuarioRepository usuarioRepository, DbEmprestimosContext context, HashingService hashingService)
        {
            _usuarioRepository = usuarioRepository;
            _context = context;
            _hashingService = hashingService;
        }

        public async Task<IEnumerable<UsuarioReadDTO>> GetAllUsers(int pageNumber, int pageSize)
        {
            var usuarios = await _usuarioRepository.GetAllUsers(pageNumber, pageSize);
            return usuarios.Select(a => new UsuarioReadDTO
            {
                IdUsuario = a.IdUsuario,
                NomeCompleto = a.NomeCompleto,
                EmailPessoal = a.EmailPessoal,
                NumeroTelefone = a.NumeroTelefone,
                SenhaHash = a.SenhaHash,
                AssociacaoNomeFantasia = a.Associacao?.NomeFantasia,
                IdAssociacao = a.Associacao?.IdAssociacao ?? 0
            }).ToList();
        }

        public async Task<UsuarioReadDTO> GetUserById(int id)
        {
            var usuario = await _usuarioRepository.GetUserById(id);
            if(usuario == null) return null;

            return new UsuarioReadDTO
            {
                IdUsuario = usuario.IdUsuario,
                NomeCompleto = usuario.NomeCompleto,
                EmailPessoal = usuario.EmailPessoal,
                NumeroTelefone = usuario.NumeroTelefone,
                SenhaHash = usuario.SenhaHash,
                AssociacaoNomeFantasia = usuario.Associacao.NomeFantasia,
                IdAssociacao = usuario.Associacao.IdAssociacao
            };
        }

        public async Task<UsuarioReadDTO> AddUser(UsuarioCreateDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                Cpf = usuarioDTO.Cpf,
                NomeCompleto = usuarioDTO.NomeCompleto,
                NumeroTelefone = usuarioDTO.NumeroTelefone,
                EmailPessoal = usuarioDTO.EmailPessoal,
                SenhaHash = usuarioDTO.Senha,
                DataNascimento = usuarioDTO.DataNascimento,
                Endereco = usuarioDTO.Endereco,
                IdAssociacao = usuarioDTO.IdAssociacao
            };

            usuario.SenhaHash = _hashingService.HashPassword(usuario, usuarioDTO.Senha);

            var newUsuario = await _usuarioRepository.AddUser(usuarioDTO);

            if (newUsuario.Associacao == null)
            {
                await _context.Entry(newUsuario).Reference(u => u.Associacao).LoadAsync();
            }

            return new UsuarioReadDTO
            {
                IdUsuario = newUsuario.IdUsuario,
                NomeCompleto = newUsuario.NomeCompleto,
                NumeroTelefone = newUsuario.NumeroTelefone,
                EmailPessoal = newUsuario.EmailPessoal,
                SenhaHash = newUsuario.SenhaHash,
                AssociacaoNomeFantasia = newUsuario.Associacao?.NomeFantasia ?? "Desconhecida",
                IdAssociacao = newUsuario.Associacao?.IdAssociacao ?? 0
            };
        }

        public async Task UpdateUser(int id, UsuarioUpdateDTO usuarioDTO)
        {
            var usuario = await _usuarioRepository.GetUserById(id);
            if(usuario == null)
            {
                throw new KeyNotFoundException("Usuário Not Found");
            }

            usuario.Cpf = usuarioDTO.Cpf;
            usuario.NomeCompleto = usuarioDTO.NomeCompleto;
            usuario.NumeroTelefone = usuarioDTO.NumeroTelefone;
            usuario.EmailPessoal = usuarioDTO.EmailPessoal;
            if (!string.IsNullOrEmpty(usuarioDTO.Senha))
            {
                usuario.SenhaHash = _hashingService.HashPassword(usuario, usuarioDTO.Senha);
            }
            usuario.DataNascimento = usuarioDTO.DataNascimento;
            usuario.Endereco = usuarioDTO.Endereco;

            await _usuarioRepository.UpdateUser(usuario);

        }

        public async Task DeleteUser(int id)
        {
            var usuario = await _usuarioRepository.GetUserById(id);

            if(usuario == null)
            {
                throw new KeyNotFoundException("Usuário Not Found");
            }

            await _usuarioRepository.DeleteUser(id);
        }

        public async Task ChangeUserPassword(int id, string newPassword)
        {
            var user = await _usuarioRepository.GetUserById(id);
            if(user != null)
            {
                user.SenhaHash = _hashingService.HashPassword(user, newPassword);
                await _usuarioRepository.UpdateUser(user);
            }
        }
    }
}
