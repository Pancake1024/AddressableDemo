namespace Party
{
    public interface IAssetLoader
    {
        int Priority { get; }

        IAssetLoader InitLoader(string path, int priority, IAssetManager assetManager, System.Action<UnityEngine.Object> callback);

        void Release();

        bool Update();
        
    }
}