﻿using EmprestimosAPI.DTO.Emprestimo;

namespace EmprestimosAPI.DTO.Pessoa
{
    public class PessoaReadDTO
    {
        public int IdPessoa { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Descricao { get; set; }
        public string Endereco { get; set; }
        public int StatusEmprestimo { get; set; }
        public DateTime? DataEmprestimo { get; set; }
        public string Foto1Pessoa { get; set; }
        public string Foto2Pessoa { get; set; }
    }
}
