using EmprestimosAPI.DTO.Usuario;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmprestimosAPI.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioReadDTO>> GetAllUsers(int pageNumber, int pageSize);
        Task<UsuarioReadDTO> GetUserById(int id);
        Task<UsuarioReadDTO> AddUser(UsuarioCreateDTO usuarioDTO);
        Task UpdateUser(int id, UsuarioUpdateDTO usuarioDTO);
        Task DeleteUser(int id);
    }
}

