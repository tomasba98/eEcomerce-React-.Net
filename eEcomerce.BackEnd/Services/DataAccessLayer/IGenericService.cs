using eEcomerce.BackEnd.Entities;

using System.Linq.Expressions;

namespace eEcomerce.BackEnd.Services.DataAccessLayer
{
    /// <summary>
    /// Generic Service interface for performing business logic and CRUD operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGenericService<TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Asynchronously inserts a single entity into the database.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        Task InsertAsync(TEntity entity);

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
        /// Asynchronously retrieves all entities from the database.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        Task<IEnumerable<TEntity>> FindAllAsync();

        /// <summary>
        /// Retrieves entities that match the specified expression.
        /// </summary>
        /// <param name="expression">The expression to filter entities.</param>
        /// <returns>A queryable collection of entities that match the expression.</returns>
        IQueryable<TEntity> FilterByExpressionLinq(Expression<Func<TEntity, bool>> expression);
    }
}