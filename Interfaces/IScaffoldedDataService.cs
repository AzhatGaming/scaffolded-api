namespace ScaffoldedApi.Interfaces
{
    public interface IScaffoldedDataService<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync(IDictionary<string, object> filters,  CancellationToken cancellationToken = default);
        public Task<T> GetAsync(string keys, CancellationToken cancellationToken = default);
        public Task<T> AddAsync(T model, CancellationToken cancellationToken = default);
        public Task<T> UpdateAsync(string keys, T model, CancellationToken cancellationToken = default);
        public Task DeleteAsync(string keys, CancellationToken cancellationToken = default);
    }
}
