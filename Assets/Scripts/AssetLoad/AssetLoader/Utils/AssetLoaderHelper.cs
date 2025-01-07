using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Party
{
    public static class AssetLoaderHelper
    {  
        private static List<IAssetLoader> _PendingAssetLoader = new List<IAssetLoader>();
        private static List<IAssetLoader> _CreatedAssetLoader = new List<IAssetLoader>(2);
        
        public static List<IAssetLoader> PendingAssetLoader => _PendingAssetLoader;

        public static void AddLoaderWrapper(ILoaderWrapper loaderWrapper,Action<Object> callBack)
        {
            AssetLoaderFactory.CreateAssetLoader(loaderWrapper, AssetManager.Instance, callBack, _CreatedAssetLoader);
            
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
    }
}