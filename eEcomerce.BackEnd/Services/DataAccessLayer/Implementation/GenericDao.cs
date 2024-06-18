using eEcomerce.BackEnd.Context;
using eEcomerce.BackEnd.Entities;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace eEcomerce.BackEnd.Services.DataAccessLayer.Implementation
{
    public class GenericDao<TEntity> : IGenericDao<TEntity> where TEntity : EntityBase
    {
        public GenericDao(AppDbContext context)
        {
            GetContext = context;
            DbSet = GetContext.Set<TEntity>();
        }

        public DbSet<TEntity> DbSet { get; }

        public AppDbContext GetContext { get; }

        public async Task InsertAsync(TEntity entity)
        {
            try
            {
                await DbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", ex);
                throw new InvalidOperationException("An error occurred while inserting the entity.", ex);
            }
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await DbSet.AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", ex);
                throw new InvalidOperationException("An error occurred while inserting the enumerable entities.", ex);
            }
        }

        public Task DeleteAsync(TEntity entity)
        {
            try
            {
                DbSet.Remove(entity);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", ex);
                throw new InvalidOperationException("An error occurred while deleting the entity.", ex);
            }
        }

        public Task UpdateAsync(TEntity entity)
        {
            try
            {
                GetContext.Entry(entity).State = EntityState.Modified;
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", ex);
                throw new InvalidOperationException("An error occurred while updating the entity.", ex);
            }
        }

        public async Task<TEntity> GetAsync(params object[] keyValues)
        {
            try
            {
                TEntity? entity = await DbSet.FindAsync(keyValues);
                return entity ?? throw new InvalidOperationException("Entity not found.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", ex);
                throw new InvalidOperationException("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            try
            {
                return await DbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", ex);
                throw new InvalidOperationException("An error occurred while retrieving all entities.", ex);
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await GetContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: {0}", ex);
                throw new InvalidOperationException("An error occurred while saving changes to the database.", ex);
            }
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Where(expression);
        }
    }
}
