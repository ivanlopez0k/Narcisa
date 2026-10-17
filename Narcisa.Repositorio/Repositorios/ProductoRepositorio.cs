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
                Activo = p.Activo
            };
        }
    }
}
