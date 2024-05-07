using eEcomerce.BackEnd.Data;
using eEcomerce.BackEnd.Entities;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace eEcomerce.BackEnd.Services.DataAccesLayer.Implementation.GenericDao;

using eEcomerce.BackEnd.Services.Services.DataAccessLayer.IGenericDao;
public class GenericDao<TEntity> : IGenericDao<TEntity> where TEntity : EntityBase
{
    public GenericDao(AppDbContext context)
    {
        GetContext = context;
        DbSet = GetContext.Set<TEntity>();
    }

    public DbSet<TEntity> DbSet { get; }

    public AppDbContext GetContext { get; }

    public void Insert(TEntity entity)
    {
        try
        {
            DbSet.Add(entity);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception: {0}", ex);
            throw;
        }
    }

    public void Insert(IEnumerable<TEntity> entities)
    {
        try
        {
            DbSet.AddRange(entities);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception: {0}", ex);
            throw;
        }
    }

    public Task InsertAsync(IEnumerable<TEntity> entities)
    {
        return DbSet.AddRangeAsync(entities);
    }

    public async Task InsertAsync(TEntity entity)
    {
        try
        {
            await DbSet.AddAsync(entity);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception: {0}", ex);
            throw;
        }
    }



    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public void Delete(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public void DeleteUnattached(TEntity entity)
    {
        DbSet.Attach(entity);
        DbSet.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        GetContext.Entry(entity).State = EntityState.Modified;
    }

    public virtual TEntity Get(params object[] keyValues)
    {
        TEntity? entity = DbSet.Find(keyValues);

        return entity ?? throw new InvalidOperationException("Entity not found.");
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public IQueryable<TEntity> FindAllQueryable()
    {
        return DbSet;
    }

    public IEnumerable<TEntity> FindAllEnumerable()
    {
        return DbSet.ToList();
    }

    public virtual IQueryable<TEntity> Include(Expression<Func<TEntity, object>> expression)
    {
        return DbSet.Include(expression);
    }

    public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        return DbSet.Where(expression);
    }

    public async Task SaveAsync()
    {
        await GetContext.SaveChangesAsync();
    }

    public void Save()
    {
        GetContext.SaveChanges();
    }

    public void GenericEntityDispose()
    {
        GetContext.Dispose();
    }
}
