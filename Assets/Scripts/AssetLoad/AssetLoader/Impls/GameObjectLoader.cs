using UnityEngine;

namespace Party
{
    public class GameObjectLoader : AbstractAssetLoader
    {
        protected override void OnStartLoad()
        {
            _AssetManager.InstantiateAsync(_Path, go =>
            {
                _IsDone = true;
                _Callback?.Invoke(go);
            });
        }
    }
}