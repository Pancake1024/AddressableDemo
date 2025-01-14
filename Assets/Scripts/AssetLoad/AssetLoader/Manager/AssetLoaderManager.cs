using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Party
{
    /// <summary>
    /// 外对调用资源加载的唯一入口
    /// </summary>
    public partial class AssetLoaderManager : SingletonMonoBehaviour<AssetLoaderManager>
    {
        [SerializeField]
        [Header("同时加载的AssetLoader数量")]
        private int PARALLEL_MAX_LOADERS_COUNT = 5;
      
        [SerializeField]
        [Header("开启资源按需动态加载功能")]
        private bool _OpenUpdate = true;
        
        private IAssetManager _AssetManager;
        private WrapperLoaderCacheManager _WrapperLoaderCacheManager;
        private List<IAssetLoader> _AssetLoaders;


        protected override void Awake()
        {
            base.Awake();
            
            _AssetManager = new AssetManager();
            _WrapperLoaderCacheManager = new WrapperLoaderCacheManager(_AssetManager);
            _AssetLoaders = new List<IAssetLoader>(PARALLEL_MAX_LOADERS_COUNT);
        }
        
        public void AddLoaderWrapper(ILoaderWrapper loaderWrapper,Action<Object> callBack)
        {
            _WrapperLoaderCacheManager.AddLoaderWrapper(loaderWrapper, callBack);
        }
      
        public void Update()
        {
            if (!_OpenUpdate) return;
            
            _UpdateAssetLoaders(Time.deltaTime);
            _UpdateAutoReleaseAsset(Time.deltaTime);
        }
    }
}