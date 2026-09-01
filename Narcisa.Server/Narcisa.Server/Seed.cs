using Narcisa.BD.Datos;
using Narcisa.BD.Datos.Entity;
using System;
using System.Linq;

namespace Narcisa.Server
{
    public static class Seed
    {
        public static void Iniciar(AppDbContext db)
        {
            if (!db.Usuario.Any())
            {
                db.Usuario.Add(new Usuario
                {
                    Nombre = "Cliente Demo",
                    email = "cliente@demo.com",
                    password = "123456",
                    rol = "Cliente",
                    createdAt = DateTime.UtcNow
                });
            }

            if (!db.Productos.Any())
            {
                db.Productos.AddRange(
                    new Producto { Nombre = "Espejo circular dorado", Descripcion = "Espejo decorativo con marco dorado.", Precio = 12000m, Stock = 10, ImagenUrl = "", Activo = true },
                    new Producto { Nombre = "Mesa ratona de madera", Descripcion = "Mesa ratona en madera natural.", Precio = 45000m, Stock = 5, ImagenUrl = "", Activo = true },
                    new Producto { Nombre = "Lampara de pie minimalista", Descripcion = "Lampara de pie estilo nordico.", Precio = 28000m, Stock = 8, ImagenUrl = "", Activo = true },
                    new Producto { Nombre = "Planta artificial eucalipto", Descripcion = "Planta ornamental para interiores.", Precio = 6500m, Stock = 20, ImagenUrl = "", Activo = true },
                    new Producto { Nombre = "Set de velas aromaticas", Descripcion = "Set de tres velas de soja.", Precio = 9800m, Stock = 15, ImagenUrl = "", Activo = true }
                );
            }

            db.SaveChanges();
        }
    }
}
