namespace Party
{
    public class MaterialAssetLoader : AbstractAssetLoader
    {
        protected override void OnStartLoad()
        {
            _AssetManager.LoadMaterialAsync(_Path, material =>
            {
                _IsDone = true;
                _Callback?.Invoke(material);
            });
        }
    }
}