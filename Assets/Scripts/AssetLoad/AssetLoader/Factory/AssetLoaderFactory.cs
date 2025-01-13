using System;
using UnityEngine;

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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loaderWrapper"></param>
        /// <param name="assetManager"></param>
        /// <param name="callBack"></param>
        /// <param name="assetLoaders"></param>
        public IAssetLoader CreateAssetLoader(LoaderData data,IAssetManager assetManager)
        {
            //TODO:如果资源精度最高的已经下载了，那么就不需要再下载低精度的资源
            //TODO:使用assetManager.GetAssetLoadStatus()方法来查询资源的加载状态
            if (data.LoaderWrapper is ImageLoaderWrapper)
            {
                return _ImageAssetLoaderPool.Borrow().InitLoader(data.Path, data.Priority, assetManager, data.CallBack);
            }
            
            if (data.LoaderWrapper is RawImageLoaderWrapper)
            {
                return _RawImageAssetLoaderPool.Borrow().InitLoader(data.Path, data.Priority, assetManager, data.CallBack);
            }
            
            if (data.LoaderWrapper is MeshLoaderWrapper)
            {
                return  _MeshAssetLoaderPool.Borrow().InitLoader(data.Path, data.Priority, assetManager, data.CallBack);
            }
            
            if (data.LoaderWrapper is GameObjectLoaderWrapper)
            {
                return _GameObjectLoaderPool.Borrow().InitLoader(data.Path, data.Priority, assetManager, data.CallBack);
            }
            
            Debug.LogError($"CreateAssetLoader error:{data.LoaderWrapper.GetType().Name}");
            return null;
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
    }
}