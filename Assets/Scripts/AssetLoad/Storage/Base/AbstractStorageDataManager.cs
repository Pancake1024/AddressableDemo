using System.Collections.Generic;

namespace Party.Storage
{
    public abstract class AbstractStorageDataManager : IStorageDataManager
    {
        protected Dictionary<string, string> _ABKey2HashDict = new Dictionary<string, string>();
        
        public void ReadABHashFileFromLocalStorage()
        {
            OnReadABHashFileFromLocalStorage();
        }

        public void UpdateABHash(string key, string newHash)
        {
            if (_ABKey2HashDict.ContainsKey(key))
            {
                _ABKey2HashDict[key] = newHash;
            }
            else
            {
                _ABKey2HashDict.Add(key, newHash);
            }
            
            OnUpdateABHash(key, newHash);
        }

        public void SaveABHashFileToLocalStorage()
        {
            OnSaveABHashFileToLocalStorage();
        }

        public string GetABHash(string key)
        {
            if (_ABKey2HashDict.ContainsKey(key))
            {
                return _ABKey2HashDict[key];
            }

            return string.Empty;
        }
        
        protected abstract void OnReadABHashFileFromLocalStorage();
        protected abstract void OnUpdateABHash(string key, string newHash);
        protected abstract void OnSaveABHashFileToLocalStorage();
    }
}