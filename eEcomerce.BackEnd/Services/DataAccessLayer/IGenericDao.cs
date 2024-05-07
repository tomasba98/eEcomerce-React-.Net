using eEcomerce.BackEnd.Data;
using eEcomerce.BackEnd.Entities;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace eEcomerce.BackEnd.Services.Services.DataAccessLayer.IGenericDao;

/// <summary>
/// Generic data access interface implementation for entities.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public interface IGenericDao<TEntity> where TEntity : EntityBase
{
    /// <summary>
    /// Gets the DbSet for entities of type TEntity in the database context.
    /// </summary>
    DbSet<TEntity> DbSet { get; }

    /// <summary>
    /// Gets the database context associated with this DAO.
    /// </summary>
    AppDbContext GetContext { get; }

    /// <summary>
    /// Retrieves all entities of a specific type from a data source without including related navigation properties.
    /// </summary>
    /// <returns>An IQueryable containing all retrieved entities.</returns>
    IQueryable<TEntity> FindAllQueryable();

    /// <summary>
    /// Deletes an entity from the data store.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Deletes multiple entities from the data store.
    /// </summary>
    /// <param name="entities">The collection of entities to delete.</param>
    void Delete(IEnumerable<TEntity> entities);

    /// <summary>
    /// Deletes an unattached entity from the data store.
    /// </summary>
    /// <param name="entity">The unattached entity to delete.</param>
    void DeleteUnattached(TEntity entity);

    /// <summary>
    /// Releases resources associated with the generic entity.
    /// </summary>
    void GenericEntityDispose();

    /// <summary>
    /// Retrieves all entities from the data store.
    /// </summary>
    /// <returns>A collection of entities.</returns>
    IEnumerable<TEntity> FindAllEnumerable();

    /// <summary>
    /// Retrieves all entities from the data store asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a collection of entities.</returns>
    Task<IEnumerable<TEntity>> FindAllAsync();

    /// <summary>
    /// Retrieves an entity by its primary key values.
    /// </summary>
    /// <param name="keyValues">The primary key values of the entity.</param>
    /// <returns>The retrieved entity.</returns>
    TEntity Get(params object[] keyValues);

    /// <summary>
    /// Retrieves an IQueryable containing included related entities based on the specified expression.
    /// </summary>
    /// <param name="expression">The expression specifying related entities to include.</param>
    /// <returns>An IQueryable with included related entities.</returns>
    IQueryable<TEntity> Include(Expression<Func<TEntity, object>> expression);

    /// <summary>
    /// Inserts an entity into the data store.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    void Insert(TEntity entity);

    /// <summary>
    /// Inserts multiple entities into the data store.
    /// </summary>
    /// <param name="entities">The collection of entities to insert.</param>
    void Insert(IEnumerable<TEntity> entities);

    /// <summary>
    /// Inserts multiple entities into the data store asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to insert.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InsertAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Inserts an entity into the data store asynchronously.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InsertAsync(TEntity entity);


    /// <summary>
    /// Saves changes to the data store.
    /// </summary>
    void Save();

    /// <summary>
    /// Saves changes to the data store asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous save operation.</returns>
    Task SaveAsync();

    /// <summary>
    /// Updates an entity in the data store.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Retrieves an IQueryable containing filtered entities based on the specified expression.
    /// </summary>
    /// <param name="expression">The filter expression.</param>
    /// <returns>An IQueryable containing filtered entities.</returns>
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
}
