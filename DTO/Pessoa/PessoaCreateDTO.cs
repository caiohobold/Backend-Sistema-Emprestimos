﻿namespace EmprestimosAPI.DTO.Pessoa
{
    public class PessoaCreateDTO
    {
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Descricao { get; set; }
        public string Endereco { get; set; }
    }
}
