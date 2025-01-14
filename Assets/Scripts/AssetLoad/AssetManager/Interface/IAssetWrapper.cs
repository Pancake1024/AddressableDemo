namespace Party
{
    public interface IAssetWrapper
    {
        void LoadAssetAsync<T>(string path, System.Action<T> callback) where T : UnityEngine.Object;

        LoaderStatus GetLoaderStatus();
        
        void Release();
    }
}