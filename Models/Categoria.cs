﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmprestimosAPI.Models
{
    public class Categoria
    {
        [Key]
        [Column("id")]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nome_categoria")]
        public string NomeCategoria { get; set;}
    }
}
