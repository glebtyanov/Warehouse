namespace DAL.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<T> AddAsync(T addRequest);

        Task<T?> UpdateAsync(T updateRequest);

        Task<bool> DeleteAsync(int id);
    }
}
