﻿// <auto-generated />
using System;
using EmprestimosAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmprestimosAPI.Migrations
{
    [DbContext(typeof(DbEmprestimosContext))]
    [Migration("20240523175021_AlterDateTime2")]
    partial class AlterDateTime2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EmprestimosAPI.Models.Associacao", b =>
                {
                    b.Property<int>("IdAssociacao")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("idassociacao");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdAssociacao"));

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("cnpj");

                    b.Property<string>("EmailProfissional")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("emailprofissional");

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("endereco");

                    b.Property<string>("NomeFantasia")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("nomefantasia");

                    b.Property<string>("NumeroTelefone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("numero_telefone");

                    b.Property<string>("RazaoSocial")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("razaosocial");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("senha");

                    b.HasKey("IdAssociacao");

                    b.ToTable("Associacoes", (string)null);
                });

            modelBuilder.Entity("EmprestimosAPI.Models.Categoria", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdCategoria"));

                    b.Property<string>("NomeCategoria")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("nome_categoria");

                    b.HasKey("IdCategoria");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("EmprestimosAPI.Models.Emprestimo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataEmprestimo")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_emprestimo");

                    b.Property<int>("IdEquipamento")
                        .HasColumnType("integer");

                    b.Property<int>("IdPessoa")
                        .HasColumnType("integer");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IdEquipamento");

                    b.HasIndex("IdPessoa");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Emprestimos");
                });

            modelBuilder.Entity("EmprestimosAPI.Models.Equipamento", b =>
                {
                    b.Property<int>("IdEquipamento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdEquipamento"));

                    b.Property<string>("CargaEquipamento")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("carga_equipamento");

                    b.Property<string>("DescricaoEquipamento")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("character varying(600)")
                        .HasColumnName("descricao_equipamento");

                    b.Property<string>("EstadoEquipamento")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("estado_equipamento");

                    b.Property<int>("IdCategoria")
                        .HasColumnType("integer")
                        .HasColumnName("id_categoria");

                    b.Property<string>("NomeEquipamento")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("nome_equipamento");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20) default 'Disponível'")
                        .HasColumnName("status");

                    b.HasKey("IdEquipamento");

                    b.HasIndex("IdCategoria");

                    b.ToTable("Equipamentos");
                });

            modelBuilder.Entity("EmprestimosAPI.Models.Pessoa", b =>
                {
                    b.Property<int>("IdPessoa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdPessoa"));

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("cpf");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(600)
                        .HasColumnType("character varying(600)")
                        .HasColumnName("descricao");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<string>("NomeCompleto")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("nome_completo");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("telefone");

                    b.HasKey("IdPessoa");

                    b.ToTable("Pessoas");
                });

            modelBuilder.Entity("EmprestimosAPI.Models.Usuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdUsuario"));

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("cpf");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("data_nascimento");

                    b.Property<string>("EmailPessoal")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email_pessoal");

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("endereco");

                    b.Property<int>("IdAssociacao")
                        .HasColumnType("integer")
                        .HasColumnName("idassociacao");

                    b.Property<string>("NomeCompleto")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("nome_completo");

                    b.Property<string>("NumeroTelefone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("numero_telefone");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("senha");

                    b.HasKey("IdUsuario");

                    b.HasIndex("IdAssociacao");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("EmprestimosAPI.Models.Emprestimo", b =>
                {
                    b.HasOne("EmprestimosAPI.Models.Equipamento", "Equipamento")
                        .WithMany()
                        .HasForeignKey("IdEquipamento")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EmprestimosAPI.Models.Pessoa", "Pessoa")
                        .WithMany()
                        .HasForeignKey("IdPessoa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EmprestimosAPI.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipamento");

                    b.Navigation("Pessoa");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("EmprestimosAPI.Models.Equipamento", b =>
                {
                    b.HasOne("EmprestimosAPI.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("EmprestimosAPI.Models.Usuario", b =>
                {
                    b.HasOne("EmprestimosAPI.Models.Associacao", "Associacao")
                        .WithMany()
                        .HasForeignKey("IdAssociacao")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Associacao");
                });
#pragma warning restore 612, 618
        }
    }
}
