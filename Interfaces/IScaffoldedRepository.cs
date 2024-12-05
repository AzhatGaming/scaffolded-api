namespace ScaffoldedApi.Interfaces
{
    public interface IScaffoldedRepository<T> where T : class
    {
        public IQueryable<T> GetAll();
        public Task<T> GetAsync(object[] keys, CancellationToken cancellationToken = default);
        public Task<T> AddAsync(T model, CancellationToken cancellationToken = default);
        public Task<T> UpdateAsync(object[] keys, T model, CancellationToken cancellationToken = default);
        public Task DeleteAsync(object[] keys, CancellationToken cancellationToken = default);
    }
}
