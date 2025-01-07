namespace Party
{
    public class Texture2DAssetLoader : AbstractAssetLoader
    {
        protected override void OnStartLoad()
        {
            _AssetManager.LoadTexture2DAsync(_Path, texture =>
            {
                _IsDone = true;
                _Callback?.Invoke(texture);
            });
        }
    }
}