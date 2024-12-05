using Microsoft.EntityFrameworkCore;
using ScaffoldedApi.Interfaces;
using ScaffoldedApi.QueryFilter;
using ScaffoldedApi.Utilities;

namespace ScaffoldedApi
{
    public class ScaffoldedDataService<T> : IScaffoldedDataService<T> where T : class
    {
        protected IScaffoldedRepository<T> _repository;

        public ScaffoldedDataService(IScaffoldedRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(IDictionary<string, object> filters, CancellationToken cancellationToken = default)
        {
            var query = _repository.GetAll();

            foreach (var filter in filters.GetQueryFilters<T>())
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<T> GetAsync(string keys, CancellationToken cancellationToken = default)
        {
            return await _repository.GetAsync(keys.ToKeyValues<T>(), cancellationToken);
        }

        public virtual async Task<T> AddAsync(T model, CancellationToken cancellationToken = default)
        {
            return await _repository.AddAsync(model, cancellationToken);
        }

        public virtual async Task<T> UpdateAsync(string keys, T model, CancellationToken cancellationToken = default)
        {
            return await _repository.UpdateAsync(keys.ToKeyValues<T>(), model, cancellationToken);
        }

        public virtual async Task DeleteAsync(string keys, CancellationToken cancellationToken = default)
        {
            await _repository.DeleteAsync(keys.ToKeyValues<T>(), cancellationToken);
        }
    }
}
