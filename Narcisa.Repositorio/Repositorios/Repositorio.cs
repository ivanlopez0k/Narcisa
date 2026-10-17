using Microsoft.EntityFrameworkCore;
using Narcisa.BD.Datos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narcisa.Repositorio.Repositorios
{
    public class Repositorio<E> : IRepositorio<E> where E : class, IEntityBase
    {
        protected readonly AppDbContext context;

        public Repositorio(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Existe(E entity)
        {
            return await context.Set<E>().AnyAsync(e => e.Id == entity.Id);
        }

        public async Task<List<E>> Select()
        {
            return await context.Set<E>().ToListAsync();
        }

        public async Task<E?> SelectById(int id)
        {
            return await context.Set<E>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<E> Insert(E entity)
        {
            await context.Set<E>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Update(E entity)
        {
            bool existe = await context.Set<E>().AnyAsync(e => e.Id == entity.Id);
            if (!existe)
            {
                return false;
            }

            context.Set<E>().Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var entidad = await context.Set<E>().FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return false;
            }

            context.Set<E>().Remove(entidad);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
