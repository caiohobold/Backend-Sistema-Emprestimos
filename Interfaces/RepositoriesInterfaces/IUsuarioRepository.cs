using EmprestimosAPI.DTO.Usuario;
using EmprestimosAPI.Helpers;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface IUsuarioRepository
    {
        Task<PagedList<Usuario>> GetAllUsers(int pageNumber, int pageSize);
        Task<Usuario> GetUserById(int id);
        Task<Usuario> GetUserByEmailAsync(string email);
        Task<Usuario> AddUser(UsuarioCreateDTO usuarioDTO);
        Task UpdateUser(Usuario usuario);
        Task DeleteUser(int id);
    }
}
