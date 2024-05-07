using eEcomerce.BackEnd.Data;
using eEcomerce.BackEnd.Entities;

using System.Linq.Expressions;

namespace eEcomerce.BackEnd.Services.DataAccesLayer.Implementation.GenericService;

using eEcomerce.BackEnd.Services.DataAccesLayer.Implementation.GenericDao;
using eEcomerce.BackEnd.Services.DataAccessLayer.IGenericService;
public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : EntityBase
{
    protected GenericDao<TEntity> genericDao;

    public GenericService(AppDbContext context)
    {
        genericDao = new GenericDao<TEntity>(context);
    }

    public virtual void Insert(TEntity entity)
    {
        genericDao.Insert(entity);
        genericDao.Save();
    }

    public virtual void Insert(ICollection<TEntity> entities)
    {
        genericDao.Insert(entities);
        genericDao.Save();
    }

    public virtual IQueryable<TEntity> FindAllQueryable()
    {
        return genericDao.FindAllQueryable();
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        await genericDao.InsertAsync(entity);
        await genericDao.SaveAsync();
    }

    public virtual async Task InsertAsync(ICollection<TEntity> entities)
    {
        await genericDao.InsertAsync(entities);
        await genericDao.SaveAsync();
    }

    public virtual void Delete(TEntity entity)
    {
        genericDao.Delete(entity);
        genericDao.Save();
    }

    public virtual void Delete(IEnumerable<TEntity> entities)
    {
        foreach (TEntity item in entities)
        {
            genericDao.Delete(item);
        }
        genericDao.Save();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        genericDao.Delete(entity);
        await genericDao.SaveAsync();
    }

    public virtual async Task DeleteWhereAsync(Expression<Func<TEntity, bool>> expression)
    {
        IQueryable<TEntity> entities = genericDao.Where(expression);
        genericDao.Delete(entities);
        await genericDao.SaveAsync();
    }

    public virtual void DeleteUnattached(TEntity entity)
    {
        genericDao.DeleteUnattached(entity);
        genericDao.Save();
    }

    public virtual void Update(TEntity entity)
    {
        genericDao.Update(entity);
        genericDao.Save();
    }

    public virtual Task UpdateAsync(TEntity entity)
    {
        genericDao.Update(entity);
        return genericDao.SaveAsync();
    }

    public virtual TEntity? TryGet(params object[] keyValues)
    {
        try
        {
            return genericDao.Get(keyValues);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> FindAllAsync()
    {
        return await genericDao.FindAllAsync();
    }

    public virtual IEnumerable<TEntity> FindAll()
    {
        return genericDao.FindAllEnumerable();
    }

    public virtual void EntityDispose()
    {
        genericDao.GenericEntityDispose();
    }

    public IQueryable<TEntity> FilterByExpression(Expression<Func<TEntity, bool>> expression)
    {
        return genericDao.Where(expression);
    }
}