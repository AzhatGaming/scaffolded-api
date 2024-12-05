using Microsoft.EntityFrameworkCore;
using ScaffoldedApi.Interfaces;

namespace ScaffoldedApi
{
    public class ScaffoldedRepository<T> : IScaffoldedRepository<T> where T : class
    {
        protected readonly DbContext _context;

        public ScaffoldedRepository(Func<Type, DbContext> contextResolver)
        {
            _context = contextResolver(typeof(T));
        }

        public virtual IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual async Task<T> GetAsync(object[] keys, CancellationToken cancellationToken = default)
        {
            var entity = await _context.FindAsync<T>(keys, cancellationToken);

            return entity is null ? throw new ArgumentException($"Entity of type {typeof(T).Name} with key value(s) ({string.Join(", ", keys.Select(k => $"{k}"))}) could not be found.") : entity;
        }

        public virtual async Task<T> AddAsync(T model, CancellationToken cancellationToken = default)
        {
            var entry = await _context.AddAsync(model, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entry.Entity;
        }

        public virtual async Task<T> UpdateAsync(object[] keys, T model, CancellationToken cancellationToken = default)
        {
            var entity = await GetAsync(keys, cancellationToken);
            var entry = _context.Entry(entity);
            entry.CurrentValues.SetValues(model);
            entry.State = EntityState.Modified;

            await _context.SaveChangesAsync(cancellationToken);

            return entry.Entity;
        }

        public virtual async Task DeleteAsync(object[] keys, CancellationToken cancellationToken = default)
        {
            var entity = await GetAsync(keys, cancellationToken);
            var entry = _context.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
