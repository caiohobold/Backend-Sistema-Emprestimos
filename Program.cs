
using EmprestimosAPI.Data;
using EmprestimosAPI.Interfaces.RepositoriesInterfaces;
using EmprestimosAPI.Interfaces.Services;
using EmprestimosAPI.Interfaces.ServicesInterfaces;
using EmprestimosAPI.Repositories;
using EmprestimosAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using Microsoft.IdentityModel.Tokens;

namespace EmprestimosAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<IAssociacaoRepository, AssociacaoRepository>();
            builder.Services.AddScoped<IAssociacaoService, AssociacaoService>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
            builder.Services.AddScoped<IPessoaService, PessoaService>();
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<IEquipamentoRepository, EquipamentoRepository>();
            builder.Services.AddScoped<IEquipamentoService, EquipamentoService>();
            builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
            builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();
            builder.Services.AddScoped<HashingService>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddDbContext<DbEmprestimosContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbEmprestimosContext")));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
