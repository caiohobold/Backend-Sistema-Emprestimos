using EmprestimosAPI.DTO.Local;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Services
{
    public class LocalService : ILocalService
    {
        private readonly ILocalRepository _localRepository;

        public LocalService(ILocalRepository localRepository)
        {
            _localRepository = localRepository;
        }

        public async Task<IEnumerable<LocalReadDTO>> GetAllLocaisAsync(int pageNumber, int pageSize)
        {
            return await _localRepository.GetAllLocaisAsync(pageNumber, pageSize);
        }

        public async Task<LocalReadDTO> GetLocalByIdAsync(int id)
        {
            var local = await _localRepository.GetLocalByIdAsync(id);
            if (local == null) return null;

            return new LocalReadDTO
            {
                IdLocal = local.IdLocal,
                NomeLocal = local.NomeLocal
            };
        }

        public async Task<LocalReadDTO> AddLocalAsync(LocalCreateDTO localDTO)
        {
            var local = new Local
            {
                NomeLocal = localDTO.NomeLocal
            };

            var newLocal = await _localRepository.AddLocalAsync(local);

            return new LocalReadDTO
            {
                IdLocal = newLocal.IdLocal,
                NomeLocal = newLocal.NomeLocal
            };
        }

        public async Task UpdateLocalAsync(int id, LocalUpdateDTO localDTO)
        {
            var local = await _localRepository.GetLocalByIdAsync(id);
            if (local == null)
            {
                throw new KeyNotFoundException("Local não encontrado.");
            }

            local.NomeLocal = localDTO.NomeLocal;
            await _localRepository.UpdateLocalAsync(local);
        }

        public async Task DeleteLocalAsync(int id)
        {
            await _localRepository.DeleteLocalAsync(id);
        }
    }
}
