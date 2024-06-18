using eEcomerce.BackEnd.Entities;

using System.Linq.Expressions;

namespace eEcomerce.BackEnd.Services.DataAccessLayer
{
    /// <summary>
    /// Generic Data Access Object interface for performing CRUD operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGenericDao<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Asynchronously inserts a single entity into the database.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Asynchronously inserts a collection of entities into the database.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        Task InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Asynchronously deletes a single entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Asynchronously updates a single entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Asynchronously retrieves a single entity by its primary key values.
        /// </summary>
        /// <param name="keyValues">The primary key values.</param>
        /// <returns>The entity found, or throws an exception if not found.</returns>
        Task<TEntity> GetAsync(params object[] keyValues);

        /// <summary>
        /// Asynchronously retrieves all entities from the database.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        Task<IEnumerable<TEntity>> FindAllAsync();

        /// <summary>
        /// Retrieves entities that match the specified expression.
        /// </summary>
        /// <param name="expression">The expression to filter entities.</param>
        /// <returns>A queryable collection of entities that match the expression.</returns>
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Asynchronously saves all changes made in the context to the database.
        /// </summary>
        Task SaveAsync();
    }
}
