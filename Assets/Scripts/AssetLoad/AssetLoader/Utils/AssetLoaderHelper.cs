using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Party
{
    public static class AssetLoaderHelper
    {  
        private const int MAX_LOADERS_COUNT = 10;
        
        private static List<IAssetLoader> _PendingAssetLoader = new List<IAssetLoader>();
        private static List<IAssetLoader> _CreatedAssetLoader = new List<IAssetLoader>(2);
        
        private static AssetLoaderFactory _AssetLoaderFactory = new AssetLoaderFactory();
        
        private static List<LoaderWrapperData> _LoaderWrapperDatas = new List<LoaderWrapperData>(64);
        private static CSharpClassPool<LoaderWrapperData> _LoaderWrapperDataPool =
            CSharpClassPool<LoaderWrapperData>.Build(MAX_LOADERS_COUNT, MAX_LOADERS_COUNT * 2, () => new LoaderWrapperData(), data => data.Release(),
                data => data.Release(), data => data.Release());
        
        public static List<IAssetLoader> PendingAssetLoader => _PendingAssetLoader;
        public static AssetLoaderFactory AssetLoaderFactory => _AssetLoaderFactory;
        
        public static void AddLoaderWrapper(ILoaderWrapper loaderWrapper,Action<Object> callBack)
        {
            _LoaderWrapperDatas.Add(_LoaderWrapperDataPool.Borrow().Init(loaderWrapper, callBack));
            
            // _AssetLoaderFactory.CreateAssetLoader(loaderWrapper, AssetManager.Instance, callBack, _CreatedAssetLoader);
            //
            // _CreatedAssetLoader.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            //
            // for (int j = 0; j < _CreatedAssetLoader.Count; j++)
            // {
            //     var loader = _CreatedAssetLoader[j];
            //     int insertIndex = _PendingAssetLoader.FindLastIndex(x => x.Priority <= loader.Priority);
            //     if (insertIndex == -1)
            //     {
            //         _PendingAssetLoader.Insert(0, loader);
            //     }
            //     else
            //     {
            //         _PendingAssetLoader.Insert(insertIndex + 1, loader);
            //     }
            // }
            //
            // _CreatedAssetLoader.Clear();
        }

        public static void Update(int maxCount)
        {
            if (_PendingAssetLoader.Count >= maxCount || _LoaderWrapperDatas.Count == 0)
            {
                return;
            }
            
            for (int i = 0; i < _LoaderWrapperDatas.Count; i++)
            {
                var data = _LoaderWrapperDatas[i];
                _AssetLoaderFactory.CreateAssetLoader(data.LoaderWrapper, AssetManager.Instance, data.CallBack, _CreatedAssetLoader);
                _LoaderWrapperDataPool.Return(data);
                _LoaderWrapperDatas.RemoveAt(i);
                i--;
                if ((_PendingAssetLoader.Count + _CreatedAssetLoader.Count) >= maxCount)
                {
                    break;
                }
            }
               
            _CreatedAssetLoader.Sort((a, b) => a.Priority.CompareTo(b.Priority));
                   
            for (int i = 0; i < _CreatedAssetLoader.Count; i++)
            {
                var loader = _CreatedAssetLoader[i];
                int insertIndex = _PendingAssetLoader.FindLastIndex(x => x.Priority <= loader.Priority);
                if (insertIndex == -1)
                {
                    _PendingAssetLoader.Insert(0, loader);
                }
                else
                {
                    _PendingAssetLoader.Insert(insertIndex + 1, loader);
                }
            }
                   
            _CreatedAssetLoader.Clear();
        }
        
        public class LoaderWrapperData
        {
            public ILoaderWrapper LoaderWrapper;
            public Action<Object> CallBack;

            public LoaderWrapperData Init(ILoaderWrapper loaderWrapper,Action<Object> callBack)
            {
                LoaderWrapper = loaderWrapper;
                CallBack = callBack;
                return this;
            }
            
            public void Release()
            {
                LoaderWrapper = null;
                CallBack = null;
            }
        }
    }
}