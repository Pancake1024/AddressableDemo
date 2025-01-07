using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Party
{
    public class AssetLoaderFactory
    {
        private const int POOL_MIN_SIZE = 4;
        private const int POOL_MAX_SIZE = 8;
        
        private CSharpClassPool<GameObjectLoader> _GameObjectLoaderPool;
        private CSharpClassPool<ImageAssetLoader> _ImageAssetLoaderPool;
        private CSharpClassPool<MaterialAssetLoader> _MaterialLoaderPool;
        private CSharpClassPool<MeshAssetLoader> _MeshAssetLoaderPool;
        private CSharpClassPool<RawImageAssetLoader> _RawImageAssetLoaderPool;
        private CSharpClassPool<Texture2DAssetLoader> _Tex2DAssetLoaderPool;

        public AssetLoaderFactory()
        {
            _GameObjectLoaderPool = CSharpClassPool<GameObjectLoader>.Build(POOL_MIN_SIZE,POOL_MAX_SIZE, () => new GameObjectLoader(), loader=>loader.Release(), loader=>loader.Release(),loader=>loader.Release());
            _ImageAssetLoaderPool = CSharpClassPool<ImageAssetLoader>.Build(POOL_MIN_SIZE,POOL_MAX_SIZE, () => new ImageAssetLoader(), loader=>loader.Release(), loader=>loader.Release(),loader=>loader.Release());
            _MaterialLoaderPool = CSharpClassPool<MaterialAssetLoader>.Build(POOL_MIN_SIZE,POOL_MAX_SIZE, () => new MaterialAssetLoader(), loader=>loader.Release(), loader=>loader.Release(),loader=>loader.Release());
            _MeshAssetLoaderPool = CSharpClassPool<MeshAssetLoader>.Build(POOL_MIN_SIZE,POOL_MAX_SIZE, () => new MeshAssetLoader(), loader=>loader.Release(), loader=>loader.Release(),loader=>loader.Release());
            _RawImageAssetLoaderPool = CSharpClassPool<RawImageAssetLoader>.Build(POOL_MIN_SIZE,POOL_MAX_SIZE, () => new RawImageAssetLoader(), loader=>loader.Release(), loader=>loader.Release(),loader=>loader.Release());
            _Tex2DAssetLoaderPool = CSharpClassPool<Texture2DAssetLoader>.Build(POOL_MIN_SIZE,POOL_MAX_SIZE, () => new Texture2DAssetLoader(), loader=>loader.Release(), loader=>loader.Release(),loader=>loader.Release());
        }
        
        public IAssetLoader[] CreateAssetLoader(ILoaderWrapper loaderWrapper,IAssetManager assetManager,Action<Object> callBack,List<IAssetLoader> assetLoaders)
        {
            if (loaderWrapper is ImageLoaderWrapper)
            {
                assetLoaders.Add(_ImageAssetLoaderPool.Borrow().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"1"), loaderWrapper.Priority, assetManager, callBack));
                assetLoaders.Add(_ImageAssetLoaderPool.Borrow().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"2"),loaderWrapper.Priority+1, assetManager, callBack));
            }
            else if (loaderWrapper is RawImageLoaderWrapper)
            {
                assetLoaders.Add(_RawImageAssetLoaderPool.Borrow().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"1"), loaderWrapper.Priority, assetManager, callBack));
                assetLoaders.Add(_RawImageAssetLoaderPool.Borrow().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"2"),loaderWrapper.Priority+1, assetManager, callBack));
            }else if (loaderWrapper is MeshLoaderWrapper)
            {
                assetLoaders.Add(_MeshAssetLoaderPool.Borrow().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"1"), loaderWrapper.Priority, assetManager, callBack));
                assetLoaders.Add(_MeshAssetLoaderPool.Borrow().InitLoader(_GenerateAssetPath(loaderWrapper.Path,"2"),loaderWrapper.Priority+1, assetManager, callBack));
            }
            
            return assetLoaders.ToArray();
        }

        public void ReturnLoader(IAssetLoader loader)
        {
            if (loader is GameObjectLoader)
            {
                _GameObjectLoaderPool.Return(loader as GameObjectLoader);
            }else if (loader is ImageAssetLoader)
            {
                _ImageAssetLoaderPool.Return(loader as ImageAssetLoader);
            }else if (loader is MaterialAssetLoader)
            {
                _MaterialLoaderPool.Return(loader as MaterialAssetLoader);
            }else if (loader is MeshAssetLoader)
            {
                _MeshAssetLoaderPool.Return(loader as MeshAssetLoader);
            }else if (loader is RawImageAssetLoader)
            {
                _RawImageAssetLoaderPool.Return(loader as RawImageAssetLoader);
            }else if (loader is Texture2DAssetLoader)
            {
                _Tex2DAssetLoaderPool.Return(loader as Texture2DAssetLoader);
            }else
            {
                Debug.LogError($"ReturnLoader error:{loader.GetType().Name}");
            }
        }

        
        //TODO：临时处理，后续依据开发需求修改
        private string _GenerateAssetPath(string path,string level)
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