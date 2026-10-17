using Microsoft.EntityFrameworkCore;
using Narcisa.BD.Datos;
using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(AppDbContext context) : base(context)
        {
        }

        public async Task<Usuario?> ObtenerPorEmail(string email)
        {
            return await context.Usuario
                .FirstOrDefaultAsync(u => u.email.ToLower() == email.Trim().ToLower());
        }

        public async Task<Usuario?> ValidarCredenciales(string email, string password)
        {
            return await context.Usuario
                .FirstOrDefaultAsync(u => u.email.ToLower() == email.Trim().ToLower() && u.password == password);
        }

        public async Task<Usuario> Registrar(RegisterDTO dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre.Trim(),
                email = dto.Email.Trim().ToLower(),
                password = dto.Password,
                rol = string.IsNullOrWhiteSpace(dto.Rol) ? "Cliente" : dto.Rol,
                createdAt = DateTime.UtcNow
            };

            context.Usuario.Add(usuario);
            await context.SaveChangesAsync();
            return usuario;
        }
    }
}
