using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace english_learning_server.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Add entity
        /// </summary>
        void Add(TEntity entity);

        /// <summary>
        /// Add entities
        /// </summary>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Add entity async
        /// </summary>
        ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add entities
        /// </summary>
        ValueTask AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update entity
        /// </summary>
        void Update(TEntity entity);

        /// <summary>
        /// Update the properties of the entity
        /// SKip the relations
        /// </summary>
        void UpdateOwnProperties(TEntity entity);

        /// <summary>
        /// Update entities
        /// </summary>
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update the properties of the entity
        /// SKip the relations
        /// </summary>
        void UpdateOwnProperties(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        void DeleteRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes by condition
        /// </summary>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if an entity exists
        /// </summary>
        bool Exists(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Check if an entity exists
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all entities
        /// </summary>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Get entities by condition
        /// </summary>
        IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get first entity by condition
        /// </summary>
        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get first entity by condition or default if not found
        /// </summary>
        TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get first entity by condition
        /// </summary>
        Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get first entity by condition or default if not found
        /// </summary>
        Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get single entity by condition
        /// </summary>
        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get single entity by condition
        /// </summary>
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all entities by condition
        /// </summary>
        Task<List<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Count by condition
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    }
}