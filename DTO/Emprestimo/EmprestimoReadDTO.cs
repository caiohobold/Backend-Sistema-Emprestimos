namespace EmprestimosAPI.DTO.Emprestimo
{
    public class EmprestimoReadDTO
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public int IdEquipamento { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public int Status { get; set; }
        public int IdUsuario { get; set; }
    }
}
