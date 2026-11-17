using Microsoft.EntityFrameworkCore;
using Narcisa.BD.Datos;
using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public class AsesoramientoRepositorio : Repositorio<Asesoramiento>, IAsesoramientoRepositorio
    {
        public AsesoramientoRepositorio(AppDbContext context) : base(context)
        {
        }

        public async Task<List<AsesoramientoDTO>> ObtenerPorUsuario(int usuarioId)
        {
            var asesoramientos = await context.Asesoramientos
                .AsNoTracking()
                .Include(a => a.Usuario)
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return asesoramientos.Select(MapToDTO).ToList();
        }

        public async Task<List<AsesoramientoDTO>> ObtenerTodos()
        {
            var asesoramientos = await context.Asesoramientos
                .AsNoTracking()
                .Include(a => a.Usuario)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return asesoramientos.Select(MapToDTO).ToList();
        }

        public async Task<AsesoramientoDTO?> ObtenerPorId(int id)
        {
            var a = await context.Asesoramientos
                .AsNoTracking()
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (a == null)
            {
                return null;
            }

            return MapToDTO(a);
        }

        public async Task<AsesoramientoDTO> Crear(CrearAsesoramientoDTO dto, int usuarioId)
        {
            var tipoEspacio = string.IsNullOrWhiteSpace(dto.TipoEspacio) ? "Living / Sala de Estar" : dto.TipoEspacio.Trim();
            var descripcionOriginal = dto.Descripcion?.Trim() ?? string.Empty;
            var descripcionFormateada = descripcionOriginal.StartsWith($"[{tipoEspacio}]", StringComparison.OrdinalIgnoreCase)
                ? descripcionOriginal
                : $"[{tipoEspacio}] {descripcionOriginal}";

            var asesoramiento = new Asesoramiento
            {
                Descripcion = descripcionFormateada,
                Disponibilidad = dto.Disponibilidad?.Trim() ?? string.Empty,
                Estado = "Nueva",
                CreatedAt = DateTime.UtcNow,
                UsuarioId = usuarioId
            };

            context.Asesoramientos.Add(asesoramiento);
            await context.SaveChangesAsync();

            var usuario = await context.Usuario
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            return new AsesoramientoDTO
            {
                Id = asesoramiento.Id,
                Descripcion = asesoramiento.Descripcion,
                Disponibilidad = asesoramiento.Disponibilidad,
                Estado = asesoramiento.Estado,
                CreatedAt = asesoramiento.CreatedAt,
                UsuarioId = asesoramiento.UsuarioId,
                UsuarioNombre = usuario?.Nombre ?? string.Empty,
                UsuarioEmail = usuario?.email ?? string.Empty
            };
        }

        public async Task<bool> ActualizarEstado(int id, string nuevoEstado)
        {
            var asesoramiento = await context.Asesoramientos.FirstOrDefaultAsync(a => a.Id == id);
            if (asesoramiento == null)
            {
                return false;
            }

            asesoramiento.Estado = nuevoEstado;
            await context.SaveChangesAsync();
            return true;
        }

        private static AsesoramientoDTO MapToDTO(Asesoramiento a)
        {
            return new AsesoramientoDTO
            {
                Id = a.Id,
                Descripcion = a.Descripcion,
                Disponibilidad = a.Disponibilidad,
                Estado = a.Estado,
                CreatedAt = a.CreatedAt,
                UsuarioId = a.UsuarioId,
                UsuarioNombre = a.Usuario?.Nombre ?? string.Empty,
                UsuarioEmail = a.Usuario?.email ?? string.Empty
            };
        }
    }
}
