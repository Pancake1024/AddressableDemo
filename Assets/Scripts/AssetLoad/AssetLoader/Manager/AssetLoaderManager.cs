﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Party
{
    /// <summary>
    /// 提供资源相关的接口，及与资源相关循环逻辑的入口
    /// 不润许外部直接调用AssetManager的接口
    /// </summary>
    public partial class AssetLoaderManager : SingletonMonoBehaviour<AssetLoaderManager>
    {
        [SerializeField]
        [Header("同时加载的AssetLoader数量")]
        private int PARALLEL_MAX_LOADERS_COUNT = 5;
      
        private IAssetManager _AssetManager;
        private LoaderCacheManager _LoaderCacheManager;
        private List<IAssetLoader> _AssetLoaders;

        private bool _CanUpdate = false;

        protected override void Awake()
        {
            base.Awake();
            
            _AssetManager = new AssetManager();
            _LoaderCacheManager = new LoaderCacheManager(_AssetManager);
            _AssetLoaders = new List<IAssetLoader>(PARALLEL_MAX_LOADERS_COUNT);
        }
        
        public void LoadShaderVariants(string path,Action callBack)
        {
            _AssetManager.LoadShaderVariants(path, (shadervariants) =>
            {
                shadervariants.WarmUp();
                callBack?.Invoke();
            });
        }
        
        public void GetDownloadSizeAsync(string path, Action<long> callBack)
        {
            _AssetManager.GetDownloadSizeAsync(path, callBack);
        }
        
        public void AddLoaderWrapper(ILoaderWrapper loaderWrapper,Action<Object> callBack)
        {
            _LoaderCacheManager.AddLoaderWrapper(loaderWrapper, callBack);
        }
        
        public void Update()
        {
            if (!_TestCode())
            {
                return;
            }
            
            _UpdateAssetLoaders();
        }

        private bool _TestCode()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                _AssetManager.ReleaseAll();
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _CanUpdate = !_CanUpdate;
            }

            return _CanUpdate;
        }
    }
}