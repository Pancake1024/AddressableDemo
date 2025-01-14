using System.Collections.Generic;
using UnityEngine;

namespace Party
{
    /// <summary>
    /// 游戏中，低精度资源的自动释放
    /// 防止同种资源的各种精度资源同时存在，导致内存占用过高的问题
    /// </summary>
    public partial class AssetLoaderManager
    {
        [SerializeField]
        [Header("资源自动释放检测间隔时间")]
        private float _AssetAutoReleaseInterval = 10;
        
        [SerializeField]
        [Header("开启资源自动释放功能")]
        private bool _OpenAutoRelease = true;

        private float _AutoReleaseTime;
        private List<string> _ReleasePathList = new List<string>(10);
        private List<string> _RemoveList = new List<string>(10);
        
        private void _UpdateAutoReleaseAsset(float deltaTime)
        {
            if (!_OpenAutoRelease) return;
            
            _AutoReleaseTime += deltaTime;
            if (_AutoReleaseTime < _AssetAutoReleaseInterval) return;
            _AutoReleaseTime = 0;

            foreach (var kv in _WrapperLoaderCacheManager.Path2AssetPath)
            {
                var maxLoadStatus = _AssetManager.GetAssetLoadStatus(kv.Value.MaxPath);
                if (maxLoadStatus == LoaderStatus.Loaded && kv.Value.MaxAssetStatus != AssetStatus.Release)
                {
                    kv.Value.MaxAssetStatus = AssetStatus.Loaded;
                }

                var minLoadStatus = _AssetManager.GetAssetLoadStatus(kv.Value.MinPath);
                if (minLoadStatus == LoaderStatus.Loaded && kv.Value.MinAssetStatus != AssetStatus.Release)
                {
                    kv.Value.MinAssetStatus = AssetStatus.Loaded;
                }
                
                var defaultLoadStatus = _AssetManager.GetAssetLoadStatus(kv.Value.DefaultPath);
                if (defaultLoadStatus == LoaderStatus.Loaded && kv.Value.DefaultAssetStatus != AssetStatus.Release)
                {
                    kv.Value.DefaultAssetStatus = AssetStatus.Loaded;
                }

                //有max资源且加载完毕
                if (kv.Value.MaxAssetStatus == AssetStatus.Loaded)
                {
                    //min资源也加载完毕，需要释放min资源
                    if (kv.Value.MinAssetStatus == AssetStatus.Loaded)
                    {
                        _ReleasePathList.Add(kv.Value.MinPath);
                        kv.Value.MinAssetStatus = AssetStatus.Release;
                    }
                    
                    //default资源也加载完毕，需要释放default资源
                    if (kv.Value.DefaultAssetStatus == AssetStatus.Loaded)
                    {
                        _ReleasePathList.Add(kv.Value.DefaultPath);
                        kv.Value.DefaultAssetStatus = AssetStatus.Release;
                    }

                    //max资源加载完毕，default和min资源已经释放过了或者没有参与过加载，不再需要再检查该资源了
                    if ((kv.Value.MinAssetStatus == AssetStatus.None || kv.Value.MinAssetStatus == AssetStatus.Loaded) &&
                        (kv.Value.DefaultAssetStatus == AssetStatus.None || kv.Value.DefaultAssetStatus == AssetStatus.Loaded))
                    {
                        _RemoveList.Add(kv.Key);
                    }
                    continue;
                }

                //max资源不需要加载
                if (kv.Value.MaxAssetStatus == AssetStatus.None)
                {
                    //min资源加载完毕
                    if (kv.Value.MinAssetStatus == AssetStatus.Loaded)
                    {
                        //default资源加载完毕，需要释放default资源
                        if (kv.Value.DefaultAssetStatus == AssetStatus.Loaded)
                        {
                            _ReleasePathList.Add(kv.Value.DefaultPath);
                            kv.Value.DefaultAssetStatus = AssetStatus.Release;
                        }

                        //min资源加载完毕，max没有参与加载，default资源已经释放过了或者没有参与过加载，不再需要再检查该资源了
                        if (kv.Value.DefaultAssetStatus == AssetStatus.None || kv.Value.DefaultAssetStatus == AssetStatus.Loaded)
                        {
                            _RemoveList.Add(kv.Key);
                        }
                        continue;
                    }
                }

                //max资源加载中
                if (kv.Value.MaxAssetStatus == AssetStatus.Loading)
                {
                    //min资源加载完毕
                    if (kv.Value.MinAssetStatus == AssetStatus.Loaded)
                    {
                        //default资源加载完毕，需要释放default资源
                        if (kv.Value.DefaultAssetStatus == AssetStatus.Loaded)
                        {
                            _ReleasePathList.Add(kv.Value.DefaultPath);
                            kv.Value.DefaultAssetStatus = AssetStatus.Release;
                        }
                        continue;
                    }
                }
            }
            
            foreach (var path in _ReleasePathList)
            {
                _AssetManager.ReleaseAsset(path);
            }
            _ReleasePathList.Clear();
            
            foreach (var key in _RemoveList)
            {
                _WrapperLoaderCacheManager.Path2AssetPath.Remove(key);
            }
            _RemoveList.Clear();
        }
    }
}