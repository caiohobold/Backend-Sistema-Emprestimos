namespace EmprestimosAPI.DTO.Pessoa
{
    public class PessoaUpdateDTO
    {
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string Descricao { get; set; }
        public IFormFile Foto1Pessoa { get; set; }
        public IFormFile Foto2Pessoa { get; set; }
    }
}
