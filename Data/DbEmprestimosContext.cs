using Microsoft.EntityFrameworkCore;
using EmprestimosAPI.Models;

namespace EmprestimosAPI.Data
{
    public class DbEmprestimosContext : DbContext
    {
        public DbEmprestimosContext(DbContextOptions<DbEmprestimosContext> options)
            : base (options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Associacao> Associacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Associacao>(entity =>
            {
                entity.ToTable("Associacoes");

                entity.Property(e => e.IdAssociacao).HasColumnName("idassociacao"); // Nome atual da coluna
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");

                entity.Property(e => e.IdUsuario).HasColumnName("id");
                // Outras propriedades...
                entity.Property(e => e.IdAssociacao).HasColumnName("idassociacao"); // Novo nome da coluna
                                                                                    // Configure outras propriedades e relações aqui conforme necessário
            });

            modelBuilder.Entity<Equipamento>()
            .HasOne(e => e.Categoria) // Define a relação
            .WithMany() // Define o lado inverso da relação, se aplicável
            .HasForeignKey(e => e.IdCategoria); // Chave estrangeira na tabela Equipamentos

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.DataNascimento).HasColumnType("timestamp without time zone");
            });

            modelBuilder.Entity<Emprestimo>()
                .Property(e => e.DataEmprestimo)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Unspecified))
                .HasColumnType("timestamp without time zone");

        }

    }
}
