namespace Party
{
    public class MeshAssetLoader : AbstractAssetLoader
    {
        protected override void OnStartLoad()
        {
            _AssetManager.LoadMeshAsync(_Path, mesh =>
            {
                _IsDone = true;
                _Callback?.Invoke(mesh);
            });
        }
    }
}