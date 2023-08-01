using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.EntityFramework
{
    public abstract class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        protected DbContext Context { get; set; }

        public EfEntityRepositoryBase(DbContext dbContext) { this.Context = dbContext; }

        public TEntity Add(TEntity entity)
        {
            var addedEntity = Context.Entry(entity);
            addedEntity.State = EntityState.Added;
            Context.SaveChanges();
            return addedEntity.Entity;
        }

        public void Delete(TEntity entity)
        {
            var updatedEntity = Context.Entry(entity);
            updatedEntity.State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
            var updatedEntity = Context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            Context.SaveChanges();
            return updatedEntity.Entity;
        }


        public TEntity? Get(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>()
                .SingleOrDefault(filter);
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>()
                .Where(filter)
                .ToList();
        }

        public int Count(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>()
                .Where(filter)
                .Count();
        }
    }
}

