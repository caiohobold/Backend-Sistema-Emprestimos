namespace EmprestimosAPI.Services
{
    using EmprestimosAPI.Models;
    using Microsoft.AspNetCore.Identity;
    public class HashingService
    {
        private readonly PasswordHasher<Usuario> _passwordHasher = new PasswordHasher<Usuario>();

        public string HashPassword(Usuario usuario, string password)
        {
            return _passwordHasher.HashPassword(usuario, password);
        }

        public bool VerifyPassword(Usuario usuario, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
