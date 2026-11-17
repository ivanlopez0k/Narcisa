using Microsoft.EntityFrameworkCore;
using Narcisa.BD.Datos;
using Narcisa.BD.Datos.Entity;
using Narcisa.Shared.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        public ProductoRepositorio(AppDbContext context) : base(context)
        {
        }

        public async Task<List<ProductoDTO>> ObtenerCatalogoActivo()
        {
            return await context.Productos
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .Select(p => new ProductoDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    ImagenUrl = p.ImagenUrl,
                    ImagenPngUrl = p.ImagenPngUrl,
                    Activo = p.Activo
                })
                .ToListAsync();
        }

        public async Task<ProductoDTO?> ObtenerDetalle(int id)
        {
            var p = await context.Productos.FindAsync(id);
            if (p == null || !p.Activo)
            {
                return null;
            }

            return new ProductoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                ImagenUrl = p.ImagenUrl,
                ImagenPngUrl = p.ImagenPngUrl,
                Activo = p.Activo
            };
        }

        public async Task<List<ProductoDTO>> ObtenerTodosAdmin()
        {
            return await context.Productos
                .OrderByDescending(p => p.Id)
                .Select(p => new ProductoDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    ImagenUrl = p.ImagenUrl,
                    ImagenPngUrl = p.ImagenPngUrl,
                    Activo = p.Activo
                })
                .ToListAsync();
        }

        public async Task<ProductoDTO> GuardarProducto(ProductoDTO dto)
        {
            Producto? p = null;
            if (dto.Id > 0)
            {
                p = await context.Productos.FindAsync(dto.Id);
            }

            if (p == null)
            {
                p = new Producto
                {
                    Nombre = dto.Nombre,
                    Descripcion = dto.Descripcion,
                    Precio = dto.Precio,
                    Stock = dto.Stock,
                    ImagenUrl = dto.ImagenUrl,
                    ImagenPngUrl = dto.ImagenPngUrl,
                    Activo = dto.Activo
                };
                await context.Productos.AddAsync(p);
            }
            else
            {
                p.Nombre = dto.Nombre;
                p.Descripcion = dto.Descripcion;
                p.Precio = dto.Precio;
                p.Stock = dto.Stock;
                p.ImagenUrl = dto.ImagenUrl;
                p.ImagenPngUrl = dto.ImagenPngUrl;
                p.Activo = dto.Activo;
            }

            await context.SaveChangesAsync();

            dto.Id = p.Id;
            return dto;
        }

        public async Task<bool> CambiarEstadoActivo(int id, bool activo)
        {
            var p = await context.Productos.FindAsync(id);
            if (p == null)
            {
                return false;
            }

            p.Activo = activo;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
