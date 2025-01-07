using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Party
{
    public static class AssetLoaderFactory
    {
        public static IAssetLoader[] CreateAssetLoader(ILoaderWrapper loaderWrapper,IAssetManager assetManager,Action<Object> callBack,List<IAssetLoader> assetLoaders)
        {
            if (loaderWrapper is ImageLoaderWrapper)
            {
                assetLoaders.Add(new ImageAssetLoader().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"1"), loaderWrapper.Priority, assetManager, callBack));
                assetLoaders.Add(new ImageAssetLoader().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"2"),loaderWrapper.Priority+1, assetManager, callBack));
            }
            else if (loaderWrapper is RawImageLoaderWrapper)
            {
               
                assetLoaders.Add(new RawImageAssetLoader().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"1"), loaderWrapper.Priority, assetManager, callBack));
                assetLoaders.Add(new RawImageAssetLoader().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"2"),loaderWrapper.Priority+1, assetManager, callBack));
            }else if (loaderWrapper is MeshLoaderWrapper)
            {
               
                assetLoaders.Add(new MeshAssetLoader().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"1"), loaderWrapper.Priority, assetManager, callBack));
                assetLoaders.Add(new MeshAssetLoader().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"2"),loaderWrapper.Priority+1, assetManager, callBack));
            }
            
            return assetLoaders.ToArray();
        }
        
        //TODO：临时处理，后续依据开发需求修改
        private static string _GenerateAssetPath(string path,string level)
        {
            int index = path.LastIndexOf(".", StringComparison.Ordinal);
            if (index != -1)
            {
                path = path.Insert(index, level);
            }

            return path;
        }
    }
}