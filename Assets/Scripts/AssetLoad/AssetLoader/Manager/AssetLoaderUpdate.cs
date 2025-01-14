using UnityEngine;

namespace Party
{
    /// <summary>
    /// 并行加载的AssetLoader管理
    /// </summary>
    public partial class AssetLoaderManager
    {
        [SerializeField]
        [Header("更新AssetLoader列表的间隔时间")]
        private float _UpdateLoaderTime = 0.1f;
        private float _Time;
        
        private void _UpdateAssetLoaders(float deltaTime)
        {
            _WrapperLoaderCacheManager.Update(PARALLEL_MAX_LOADERS_COUNT);

            if (_Time > _UpdateLoaderTime)
            {
                _Time = 0;
            }
            else
            {
                _Time += deltaTime;
                return;
            }
            
            int count = PARALLEL_MAX_LOADERS_COUNT - _AssetLoaders.Count;
            count = count > _WrapperLoaderCacheManager.PendingAssetLoader.Count ? _WrapperLoaderCacheManager.PendingAssetLoader.Count : count;
            for (int i = 0; i < count; i++)
            {
                var loader = _WrapperLoaderCacheManager.PendingAssetLoader[0];
                _WrapperLoaderCacheManager.PendingAssetLoader.RemoveAt(0);
                _AssetLoaders.Add(loader);
            }
            
            for (int i = 0; i < _AssetLoaders.Count; i++)
            {
                var loader = _AssetLoaders[i];
                if (loader.Update())
                {
                    _AssetLoaders.RemoveAt(i);
                    loader.Release();
                    _WrapperLoaderCacheManager.AssetLoaderFactory.ReturnLoader(loader);
                    i--;
                }
            }
        }
    }
}