using eEcomerce.BackEnd.Entities;

using System.Linq.Expressions;

namespace eEcomerce.BackEnd.Services.DataAccessLayer.IGenericService;

/// <summary>
/// Generic data access Interface for entities.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public interface IGenericService<TEntity> where TEntity : EntityBase
{
    /// <summary>
    /// Retrieves all entities as Queryable.
    /// </summary>
    /// <returns>An IQueryable containing the retrieved entities with included navigation properties.</returns>
    IQueryable<TEntity> FindAllQueryable();

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Deletes a collection of entities.
    /// </summary>
    /// <param name="entities">The collection of entities to be deleted.</param>
    void Delete(IEnumerable<TEntity> entities);

    /// <summary>
    /// Asynchronously deletes entities based on a specified condition.
    /// </summary>
    /// <param name="expression">An expression that defines the condition for selecting entities to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteWhereAsync(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// Deletes an entity from asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Deletes an unattached entity.
    /// </summary>
    /// <param name="entity">The unattached entity to delete.</param>
    void DeleteUnattached(TEntity entity);

    /// <summary>
    /// Releases resources associated with the entity.
    /// </summary>
    void EntityDispose();

    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>A collection of entities.</returns>
    IEnumerable<TEntity> FindAll();

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a collection of entities.</returns>
    Task<IEnumerable<TEntity>> FindAllAsync();

    /// <summary>
    /// Retrieves an entity based on the specified key values.
    /// </summary>
    /// <param name="keyValues">The key values that uniquely identify the entity.</param>
    /// <returns>The retrieved entity, or null if not found.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the entity is not found in the data source.</exception>
    TEntity? TryGet(params object[] keyValues);

    /// <summary>
    /// Inserts an entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    void Insert(TEntity entity);

    /// <summary>
    /// Inserts a collection of entities.
    /// </summary>
    /// <param name="entities">The collection of entities to be inserted.</param>
    void Insert(ICollection<TEntity> entities);

    /// <summary>
    /// Inserts an entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InsertAsync(TEntity entity);

    /// <summary>
    /// Asynchronously inserts a collection of entities.
    /// </summary>
    /// <param name="entities">The collection of entities to be inserted asynchronously.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InsertAsync(ICollection<TEntity> entities);


    /// <summary>
    /// Updates an entity in the data store.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Asynchronously updates an entity.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Filters entities based on the specified expression.
    /// </summary>
    /// <param name="expression">The filter expression.</param>
    /// <returns>An IQueryable containing filtered entities.</returns>
    IQueryable<TEntity> FilterByExpression(Expression<Func<TEntity, bool>> expression);
}
