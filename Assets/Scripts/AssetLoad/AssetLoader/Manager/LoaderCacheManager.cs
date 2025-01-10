using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Party
{
    /// <summary>
    /// 缓存需要动态加载的资源相关信息
    /// AssetLoaderManager有能力处理新的资源时，会从此处获取
    /// </summary>
    public class LoaderCacheManager
    {  
        private const int MAX_LOADERS_COUNT = 10;
        private IAssetManager _AssetManager;
        
        private List<IAssetLoader> _PendingAssetLoader = new List<IAssetLoader>();
        private List<IAssetLoader> _CreatedAssetLoader = new List<IAssetLoader>(2);
        
        private AssetLoaderFactory _AssetLoaderFactory = new AssetLoaderFactory();
        
        private List<LoaderData> _LoaderDatas = new List<LoaderData>(64);
        private CSharpClassPool<LoaderData> _LoaderDataPool =
            CSharpClassPool<LoaderData>.Build(MAX_LOADERS_COUNT, MAX_LOADERS_COUNT * 2, () => new LoaderData(), data => data.Release(),
                data => data.Release(), data => data.Release());
        
        public List<IAssetLoader> PendingAssetLoader => _PendingAssetLoader;
        public AssetLoaderFactory AssetLoaderFactory => _AssetLoaderFactory;
        
        public LoaderCacheManager(IAssetManager assetManager)
        {
            _AssetManager = assetManager;
        }
        
        public void AddLoaderWrapper(ILoaderWrapper loaderWrapper,Action<Object> callBack)
        {
            LoaderWrapper2LoaderData(loaderWrapper, callBack);

            _LoaderDatas.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }

        //TODO：依据后续实际规则，比如查表，或者配置文件,生成实际的Path，以及Loader的个数
        private void LoaderWrapper2LoaderData(ILoaderWrapper loaderWrapper, Action<Object> callBack)
        {
            var path = _GenerateAssetPath(loaderWrapper.Path, "1");
            var priority = loaderWrapper.Priority + 1;
            _LoaderDatas.Add(_LoaderDataPool.Borrow().Init(loaderWrapper,path, priority,callBack));
            
            path = _GenerateAssetPath(loaderWrapper.Path, "2");
            priority = loaderWrapper.Priority + 2;
            _LoaderDatas.Add(_LoaderDataPool.Borrow().Init(loaderWrapper,path, priority,callBack));
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
        
        public void Update(int maxCount)
        {
            if (_PendingAssetLoader.Count >= maxCount || _LoaderDatas.Count == 0)
            {
                return;
            }
            
            for (int i = 0; i < _LoaderDatas.Count; i++)
            {
                var data = _LoaderDatas[i];
                var loader = _AssetLoaderFactory.CreateAssetLoader(data, _AssetManager);
                if (loader != null)
                {
                    _CreatedAssetLoader.Add(loader);
                }
                _LoaderDataPool.Return(data);
                _LoaderDatas.RemoveAt(i);
                i--;
                if ((_PendingAssetLoader.Count + _CreatedAssetLoader.Count) >= maxCount)
                {
                    break;
                }
            }

            if (_CreatedAssetLoader.Count > 0)
            {
                _PendingAssetLoader.AddRange(_CreatedAssetLoader);
                _CreatedAssetLoader.Clear();
            }
        }
    }
}