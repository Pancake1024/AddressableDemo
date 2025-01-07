namespace Party
{
    public class ImageAssetLoader : AbstractAssetLoader
    {
        protected override void OnStartLoad()
        {
            _AssetManager.LoadSpriteAsync(_Path, sprite =>
            {
                _IsDone = true;
                _Callback?.Invoke(sprite);
            });
        }
    }
}