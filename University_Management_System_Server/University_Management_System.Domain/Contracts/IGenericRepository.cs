using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using University_Management_System.Domain.Entities;

namespace University_Management_System.Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntities<TKey>
    {
        // ─── Query Methods ──────────────────────────────────────────────────────
        
        /// <summary>
        /// Gets the queryable for the entity (for building custom queries)
        /// </summary>
        IQueryable<TEntity> GetQueryable();

        /// <summary>
        /// Gets all entities
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Gets all entities with optional includes
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Gets entity by ID
        /// </summary>
        Task<TEntity?> GetByIdAsync(TKey id);

        /// <summary>
        /// Gets entity by ID with optional includes
        /// </summary>
        Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Finds entities matching a predicate
        /// </summary>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Finds entities matching a predicate with optional includes
        /// </summary>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Checks if any entity matches the predicate
        /// </summary>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Counts entities matching the predicate (or all if null)
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

        // ─── Write Methods ──────────────────────────────────────────────────────

        /// <summary>
        /// Adds a new entity
        /// </summary>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Adds multiple entities
        /// </summary>
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates multiple entities
        /// </summary>
        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        Task<TEntity> DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes multiple entities
        /// </summary>
        Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}