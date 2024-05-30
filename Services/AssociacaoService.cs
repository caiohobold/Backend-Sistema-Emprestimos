using EmprestimosAPI.DTO.Associacao;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Interfaces.Services;
using EmprestimosAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmprestimosAPI.Services
{
    public class AssociacaoService : IAssociacaoService
    {
        private readonly IAssociacaoRepository _repository;

        public AssociacaoService(IAssociacaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AssociacaoReadDTO>> GetAllAsync()
        {
            var associacoes = await _repository.GetAllAssocAsync();
            return associacoes.Select(a => new AssociacaoReadDTO
            { 
                IdAssociacao = a.IdAssociacao,
                RazaoSocial = a.RazaoSocial,
                NomeFantasia = a.NomeFantasia
            }).ToList();
        }

        public async Task<AssociacaoReadDTO> GetByIdAsync(int id)
        {
            var associacao = await _repository.GetAssocById(id);
            if (associacao == null) return null;
            return new AssociacaoReadDTO
            {
                IdAssociacao = associacao.IdAssociacao,
                RazaoSocial = associacao.RazaoSocial,
                NomeFantasia = associacao.NomeFantasia
            };
        }

        public async Task<AssociacaoReadDTO> CreateAsync(AssociacaoCreateDTO associacaoDTO)
        {
            var associacao = new Associacao
            {
                EmailProfissional = associacaoDTO.EmailProfissional,
                Cnpj = associacaoDTO.CNPJ,
                RazaoSocial = associacaoDTO.RazaoSocial,
                NomeFantasia = associacaoDTO.NomeFantasia,
                NumeroTelefone = associacaoDTO.Numero_Telefone,
                Endereco = associacaoDTO.Endereco,
                Senha = associacaoDTO.Senha
            };

            var newAssociacao = await _repository.AddAssoc(associacao);

            return new AssociacaoReadDTO
            {
                IdAssociacao = newAssociacao.IdAssociacao,
                RazaoSocial = newAssociacao.RazaoSocial,
                NomeFantasia = newAssociacao.NomeFantasia
            };
        }

        public async Task UpdateAsync(int id, AssociacaoUpdateDTO associacaoDTO)
        {
            var associacao = await _repository.GetAssocById(id);
            if(associacao == null)
            {
                throw new KeyNotFoundException("Associação Not Found");
            }

            associacao.NomeFantasia = associacaoDTO.NomeFantasia;
            associacao.NumeroTelefone = associacaoDTO.Numero_Telefone;
            associacao.EmailProfissional = associacaoDTO.EmailProfissional;
            associacao.Senha = associacaoDTO.Senha;
            associacao.Endereco = associacaoDTO.Endereco;

            await _repository.UpdateAssoc(associacao);
        }

        public async Task DeleteAsync(int id)
        {
            var associacao = await _repository.GetAssocById(id);
            if (associacao == null)
                throw new KeyNotFoundException("Associação not found");

            await _repository.DeleteAssoc(id);
        }
    }
}
