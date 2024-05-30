using EmprestimosAPI.DTO.Usuario;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllUsers();
        Task<Usuario> GetUserById(int id);
        Task<Usuario> AddUser(UsuarioCreateDTO usuarioDTO);
        Task UpdateUser(Usuario usuario);
        Task DeleteUser(int id);
    }
}
