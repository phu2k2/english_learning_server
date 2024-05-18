using System.Linq.Expressions;
using english_learning_server.Data;
using english_learning_server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace english_learning_server.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected EnglishLearningDbContext _context;

        public Repository(EnglishLearningDbContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public async ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _context.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void UpdateOwnProperties(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
        }

        public void UpdateOwnProperties(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return _context.Set<TEntity>().Where(predicate).ExecuteDeleteAsync(cancellationToken);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Any(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().First(predicate);
        }

        public TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().FirstAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Single(predicate);
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().SingleAsync(predicate, cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().CountAsync(predicate, cancellationToken).ConfigureAwait(false);
        }
    }
}