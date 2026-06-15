using Microsoft.EntityFrameworkCore;
using Narcisa.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Narcisa.BD.Datos
{
    public class AppDbContext : DbContext
    {

        public DbSet<Asesoramiento> Asesoramientos { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<ProyectoProducto> ProyectosProductos{ get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
