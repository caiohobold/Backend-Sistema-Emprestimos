using EmprestimosAPI.Models;

namespace EmprestimosAPI.DTO.Equipamento
{
    public class EquipamentoReadDTO
    {
        public int IdEquipamento { get; set; }
        public string NomeEquipamento { get; set; }
        public string NomeCategoriaEquipamento { get; set; }
        public int EstadoEquipamento { get; set; }
        public int CargaEquipamento { get; set; }
        public string DescricaoEquipamento { get; set; }
        public int StatusEquipamento { get; set; }
    }
}
