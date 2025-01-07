
namespace Party.Storage
{
    public interface IStorageDataManager
    {
        void ReadABHashFileFromLocalStorage();
        
        void UpdateABHash(string key, string newHash);
        
        void SaveABHashFileToLocalStorage();
        
        string GetABHash(string key);
    }
}
