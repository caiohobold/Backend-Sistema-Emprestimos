using EmprestimosAPI.Data;
using EmprestimosAPI.DTO.Usuario;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Models;
using EmprestimosAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmprestimosAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DbEmprestimosContext _context;
        private readonly ILogger<UsuarioRepository> _logger;
        private readonly HashingService _hashingService;

        public UsuarioRepository(DbEmprestimosContext context, ILogger<UsuarioRepository> logger, HashingService hashingService)
        {
            _context = context;
            _logger = logger;
            _hashingService = hashingService;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsers()
        {
            try
            {
                return await _context.Usuarios.Include(u => u.Associacao).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllUsers: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<Usuario> GetUserById(int id)
        {
            try
            {
                var user = await _context.Usuarios
                                         .Include(u => u.Associacao)
                                         .FirstOrDefaultAsync(u => u.IdUsuario == id);

                if (user == null)
                {
                    _logger.LogError($"User with ID {id} not found.");
                }
                else if (user.Associacao == null)
                {
                    _logger.LogError($"User with ID {id} is missing association details. FK: {user.IdAssociacao}");
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserById: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<Usuario> AddUser(UsuarioCreateDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                NomeCompleto = usuarioDTO.NomeCompleto,
                EmailPessoal = usuarioDTO.EmailPessoal,
                NumeroTelefone = usuarioDTO.NumeroTelefone,
                Cpf = usuarioDTO.Cpf,
                DataNascimento = usuarioDTO.DataNascimento,
                Endereco = usuarioDTO.Endereco,
                IdAssociacao = usuarioDTO.IdAssociacao
            };

            usuario.SenhaHash = _hashingService.HashPassword(usuario, usuarioDTO.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        
        }

        public async Task UpdateUser(Usuario usuario)
        {
                
                _context.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            
        }

        public async Task DeleteUser(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);



            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            var temEmprestimo = await _context.Emprestimos
                .AnyAsync(e => e.IdUsuario == id);

            if (temEmprestimo)
            {
                throw new InvalidOperationException("Não é possível remover um usuário que está vinculado a empréstimos.");
            }
            
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
