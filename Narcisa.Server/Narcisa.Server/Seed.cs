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
            if (!db.Usuario.Any(u => u.email == "cliente@demo.com"))
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

            if (!db.Usuario.Any(u => u.email == "juan.perez@ejemplo.com"))
            {
                db.Usuario.Add(new Usuario
                {
                    Nombre = "Profesor Juan Pérez",
                    email = "juan.perez@ejemplo.com",
                    password = "password123",
                    rol = "Cliente",
                    createdAt = DateTime.UtcNow
                });
            }

            var adminUser = db.Usuario.FirstOrDefault(u => u.email == "admin@narcisa.com");
            if (adminUser == null)
            {
                db.Usuario.Add(new Usuario
                {
                    Nombre = "Administradora Narcisa",
                    email = "admin@narcisa.com",
                    password = "admin123",
                    rol = "Admin",
                    createdAt = DateTime.UtcNow
                });
            }
            else
            {
                adminUser.Nombre = "Administradora Narcisa";
                adminUser.password = "admin123";
                adminUser.rol = "Admin";
            }

            if (!db.Productos.Any())
            {
                db.Productos.AddRange(
                    new Producto
                    {
                        Nombre = "Espejo circular dorado",
                        Descripcion = "Espejo decorativo con marco dorado.",
                        Precio = 12000m,
                        Stock = 10,
                        ImagenUrl = "https://images.unsplash.com/photo-1618221195710-dd6b41faaea6?w=600&auto=format&fit=crop&q=80",
                        ImagenPngUrl = "/images/productos/espejo-dorado.png",
                        Activo = true
                    },
                    new Producto
                    {
                        Nombre = "Mesa ratona de madera",
                        Descripcion = "Mesa ratona en madera natural.",
                        Precio = 45000m,
                        Stock = 5,
                        ImagenUrl = "https://images.unsplash.com/photo-1533090481720-856c6e3c1fdc?w=600&auto=format&fit=crop&q=80",
                        ImagenPngUrl = "/images/productos/mesa-ratona.png",
                        Activo = true
                    },
                    new Producto
                    {
                        Nombre = "Lampara de pie minimalista",
                        Descripcion = "Lampara de pie estilo nordico.",
                        Precio = 28000m,
                        Stock = 8,
                        ImagenUrl = "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?w=600&auto=format&fit=crop&q=80",
                        ImagenPngUrl = "/images/productos/lampara-pie.png",
                        Activo = true
                    },
                    new Producto
                    {
                        Nombre = "Planta artificial eucalipto",
                        Descripcion = "Planta ornamental para interiores.",
                        Precio = 6500m,
                        Stock = 20,
                        ImagenUrl = "https://images.unsplash.com/photo-1512496015851-a90fb38ba796?w=600&auto=format&fit=crop&q=80",
                        ImagenPngUrl = "/images/productos/planta-eucalipto.png",
                        Activo = true
                    },
                    new Producto
                    {
                        Nombre = "Set de velas aromaticas",
                        Descripcion = "Set de tres velas de soja.",
                        Precio = 9800m,
                        Stock = 15,
                        ImagenUrl = "https://images.unsplash.com/photo-1603006905003-be475563bc59?w=600&auto=format&fit=crop&q=80",
                        ImagenPngUrl = "/images/productos/velas-aromaticas.png",
                        Activo = true
                    }
                );
            }
            else
            {
                var espejo = db.Productos.FirstOrDefault(p => p.Nombre.Contains("Espejo"));
                if (espejo != null)
                {
                    if (string.IsNullOrEmpty(espejo.ImagenUrl)) espejo.ImagenUrl = "https://images.unsplash.com/photo-1618221195710-dd6b41faaea6?w=600&auto=format&fit=crop&q=80";
                    espejo.ImagenPngUrl = "/images/productos/espejo-dorado.png";
                }

                var mesa = db.Productos.FirstOrDefault(p => p.Nombre.Contains("Mesa"));
                if (mesa != null)
                {
                    if (string.IsNullOrEmpty(mesa.ImagenUrl)) mesa.ImagenUrl = "https://images.unsplash.com/photo-1533090481720-856c6e3c1fdc?w=600&auto=format&fit=crop&q=80";
                    mesa.ImagenPngUrl = "/images/productos/mesa-ratona.png";
                }

                var lampara = db.Productos.FirstOrDefault(p => p.Nombre.Contains("Lampara") || p.Nombre.Contains("Lámpara"));
                if (lampara != null)
                {
                    if (string.IsNullOrEmpty(lampara.ImagenUrl)) lampara.ImagenUrl = "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?w=600&auto=format&fit=crop&q=80";
                    lampara.ImagenPngUrl = "/images/productos/lampara-pie.png";
                }

                var planta = db.Productos.FirstOrDefault(p => p.Nombre.Contains("Planta"));
                if (planta != null)
                {
                    if (string.IsNullOrEmpty(planta.ImagenUrl)) planta.ImagenUrl = "https://images.unsplash.com/photo-1512496015851-a90fb38ba796?w=600&auto=format&fit=crop&q=80";
                    planta.ImagenPngUrl = "/images/productos/planta-eucalipto.png";
                }

                var velas = db.Productos.FirstOrDefault(p => p.Nombre.Contains("velas"));
                if (velas != null)
                {
                    if (string.IsNullOrEmpty(velas.ImagenUrl)) velas.ImagenUrl = "https://images.unsplash.com/photo-1603006905003-be475563bc59?w=600&auto=format&fit=crop&q=80";
                    velas.ImagenPngUrl = "/images/productos/velas-aromaticas.png";
                }
            }

            db.SaveChanges();
        }
    }
}
