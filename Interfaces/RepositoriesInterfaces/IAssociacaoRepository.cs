using EmprestimosAPI.Models;

namespace EmprestimosAPI.Interfaces.RepositoriesInterfaces
{
    public interface IAssociacaoRepository
    {
        Task<IEnumerable<Associacao>> GetAllAssocAsync();
        Task<Associacao> GetAssocById(int id);
        Task<Associacao> AddAssoc(Associacao associacao);
        Task UpdateAssoc(Associacao associacao);
        Task DeleteAssoc(int id);
    }
}
