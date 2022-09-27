namespace StudyN.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(Task item);

        Task<bool> UpdateItemAsync(Task item);

        Task<bool> DeleteItemAsync(Task item);

        Task<T> GetItemAsync(bool forceRefresh = false);

        IEnumerable<T> GetItem(bool forceRefresh = false);
    }
}
