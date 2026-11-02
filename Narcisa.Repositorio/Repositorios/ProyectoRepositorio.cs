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
    public class ProyectoRepositorio : Repositorio<Proyecto>, IProyectoRepositorio
    {
        public ProyectoRepositorio(AppDbContext context) : base(context)
        {
        }

        public async Task<List<ProyectoDTO>> ObtenerPorUsuario(int usuarioId)
        {
            var proyectos = await context.Proyectos
                .AsNoTracking()
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.createdAt)
                .Include(p => p.Productos)
                    .ThenInclude(pp => pp.Producto)
                .ToListAsync();

            return proyectos.Select(p => new ProyectoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                ImagenUrl = p.ImagenUrl,
                CreatedAt = p.createdAt,
                UsuarioId = p.UsuarioId,
                Items = p.Productos.Select(pp => new ProyectoItemDTO
                {
                    Id = pp.Id,
                    ProductoId = pp.ProductoId,
                    ProductoNombre = pp.Producto?.Nombre ?? string.Empty,
                    ProductoPrecio = pp.Producto?.Precio ?? 0m,
                    ImagenPngUrl = pp.Producto?.ImagenPngUrl,
                    Posicion = pp.Posicion
                }).ToList()
            }).ToList();
        }

        public async Task<ProyectoDTO?> ObtenerDetalle(int id, int usuarioId)
        {
            var p = await context.Proyectos
                .AsNoTracking()
                .Where(p => p.Id == id && p.UsuarioId == usuarioId)
                .Include(p => p.Productos)
                    .ThenInclude(pp => pp.Producto)
                .FirstOrDefaultAsync();

            if (p == null)
            {
                return null;
            }

            return new ProyectoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                ImagenUrl = p.ImagenUrl,
                CreatedAt = p.createdAt,
                UsuarioId = p.UsuarioId,
                Items = p.Productos.Select(pp => new ProyectoItemDTO
                {
                    Id = pp.Id,
                    ProductoId = pp.ProductoId,
                    ProductoNombre = pp.Producto?.Nombre ?? string.Empty,
                    ProductoPrecio = pp.Producto?.Precio ?? 0m,
                    ImagenPngUrl = pp.Producto?.ImagenPngUrl,
                    Posicion = pp.Posicion
                }).ToList()
            };
        }

        public async Task<ProyectoDTO> GuardarProyecto(GuardarProyectoDTO dto, int usuarioId)
        {
            Proyecto proyecto;

            if (dto.Id.HasValue && dto.Id.Value > 0)
            {
                proyecto = await context.Proyectos
                    .Include(p => p.Productos)
                    .FirstOrDefaultAsync(p => p.Id == dto.Id.Value && p.UsuarioId == usuarioId)
                    ?? throw new KeyNotFoundException($"Proyecto con id {dto.Id.Value} no encontrado para este usuario.");

                proyecto.Nombre = dto.Nombre;
                proyecto.ImagenUrl = dto.ImagenUrl;

                context.ProyectosProductos.RemoveRange(proyecto.Productos);
            }
            else
            {
                proyecto = new Proyecto
                {
                    Nombre = dto.Nombre,
                    ImagenUrl = dto.ImagenUrl,
                    createdAt = DateTime.UtcNow,
                    UsuarioId = usuarioId
                };
                await context.Proyectos.AddAsync(proyecto);
                await context.SaveChangesAsync();
            }

            if (dto.Items != null && dto.Items.Count > 0)
            {
                var nuevosItems = dto.Items.Select(item => new ProyectoProducto
                {
                    ProyectoId = proyecto.Id,
                    ProductoId = item.ProductoId,
                    Posicion = item.Posicion
                }).ToList();

                await context.ProyectosProductos.AddRangeAsync(nuevosItems);
            }

            await context.SaveChangesAsync();

            var resultado = await ObtenerDetalle(proyecto.Id, usuarioId);
            return resultado ?? new ProyectoDTO
            {
                Id = proyecto.Id,
                Nombre = proyecto.Nombre,
                ImagenUrl = proyecto.ImagenUrl,
                CreatedAt = proyecto.createdAt,
                UsuarioId = proyecto.UsuarioId
            };
        }

        public async Task<bool> EliminarProyecto(int id, int usuarioId)
        {
            var proyecto = await context.Proyectos
                .Include(p => p.Productos)
                .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuarioId);

            if (proyecto == null)
            {
                return false;
            }

            context.ProyectosProductos.RemoveRange(proyecto.Productos);
            context.Proyectos.Remove(proyecto);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
