using UnityEngine;

namespace Party
{
    /// <summary>
    /// 并行加载的AssetLoader管理
    /// </summary>
    public partial class AssetLoaderManager : SingletonMonoBehaviour<AssetLoaderManager>
    {
        [SerializeField]
        [Header("更新AssetLoader列表的间隔时间")]
        private float _UpdateLoaderTime = 0.1f;
        private float _Time;
        
        private void _UpdateAssetLoaders()
        {
            _LoaderCacheManager.Update(PARALLEL_MAX_LOADERS_COUNT);

            if (_Time > _UpdateLoaderTime)
            {
                _Time = 0;
            }
            else
            {
                _Time += Time.deltaTime;
                return;
            }
            
            int count = PARALLEL_MAX_LOADERS_COUNT - _AssetLoaders.Count;
            count = count > _LoaderCacheManager.PendingAssetLoader.Count ? _LoaderCacheManager.PendingAssetLoader.Count : count;
            for (int i = 0; i < count; i++)
            {
                var loader = _LoaderCacheManager.PendingAssetLoader[0];
                _LoaderCacheManager.PendingAssetLoader.RemoveAt(0);
                _AssetLoaders.Add(loader);
            }
            
            for (int i = 0; i < _AssetLoaders.Count; i++)
            {
                var loader = _AssetLoaders[i];
                if (loader.Update())
                {
                    // Debug.LogError($"loader done:{loader.GetType().Name} {loader.Priority}");
                    _AssetLoaders.RemoveAt(i);
                    loader.Release();
                    _LoaderCacheManager.AssetLoaderFactory.ReturnLoader(loader);
                    i--;
                }
            }
        }
    }
}