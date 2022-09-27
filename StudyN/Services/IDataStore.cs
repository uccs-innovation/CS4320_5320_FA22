namespace StudyN.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);

        Task<bool> UpdateItemAsync(T item);

        Task<bool> DeleteItemAsync(int id);

        Task<T> GetItemAsync(int id);

        Task<IEnumerable<T>> GetAllItemsAsync(bool forceRefresh = false);

        IEnumerable<T> GetItems(bool forceRefresh = false);
    }
}
