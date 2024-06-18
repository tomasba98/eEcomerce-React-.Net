using eEcomerce.BackEnd.Context;
using eEcomerce.BackEnd.Entities;

using System.Linq.Expressions;

namespace eEcomerce.BackEnd.Services.DataAccessLayer.Implementation
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : EntityBase
    {
        protected IGenericDao<TEntity> GenericDao;

        public GenericService(AppDbContext context)
        {
            GenericDao = new GenericDao<TEntity>(context);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await GenericDao.InsertAsync(entity);
            await GenericDao.SaveAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            await GenericDao.DeleteAsync(entity);
            await GenericDao.SaveAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await GenericDao.UpdateAsync(entity);
            await GenericDao.SaveAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return await GenericDao.FindAllAsync();
        }

        public IQueryable<TEntity> FilterByExpressionLinq(Expression<Func<TEntity, bool>> expression)
        {
            return GenericDao.Where(expression);
        }
    }
}