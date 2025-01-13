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
        private const int CAPACITY_Asset_COUNT = 64;
        private IAssetManager _AssetManager;
        
        private List<IAssetLoader> _PendingAssetLoader = new List<IAssetLoader>();
        private List<IAssetLoader> _CreatedAssetLoader = new List<IAssetLoader>(2);
        
        private AssetLoaderFactory _AssetLoaderFactory = new AssetLoaderFactory();
        
        private List<LoaderData> _LoaderDatas = new List<LoaderData>(CAPACITY_Asset_COUNT);
        private CSharpClassPool<LoaderData> _LoaderDataPool =
            CSharpClassPool<LoaderData>.Build(MAX_LOADERS_COUNT, MAX_LOADERS_COUNT * 2, () => new LoaderData(), data => data.Release(),
                data => data.Release(), data => data.Release());
        
        private Dictionary<string,AssetPath> _Path2AssetPath = new Dictionary<string, AssetPath>(CAPACITY_Asset_COUNT);
        
        public List<IAssetLoader> PendingAssetLoader => _PendingAssetLoader;
        public AssetLoaderFactory AssetLoaderFactory => _AssetLoaderFactory;
        public Dictionary<string, AssetPath> Path2AssetPath => _Path2AssetPath;
        
        public LoaderCacheManager(IAssetManager assetManager)
        {
            _AssetManager = assetManager;
        }

        public void Clear()
        {
            _PendingAssetLoader.Clear();
            _LoaderDatas.Clear();
        }
        
        public void AddLoaderWrapper(ILoaderWrapper loaderWrapper,Action<Object> callBack)
        {
            LoaderWrapper2LoaderData(loaderWrapper, callBack);

            _LoaderDatas.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }

        //TODO：依据后续实际规则，比如查表，或者配置文件,生成实际的Path，以及Loader的个数
        private void LoaderWrapper2LoaderData(ILoaderWrapper loaderWrapper, Action<Object> callBack)
        {
            var priority = 0;
            var maxPath = Utils.GenerateAssetPath(loaderWrapper.Path, "2");
            if (!string.IsNullOrEmpty(maxPath) && _AssetManager.GetAssetLoadStatus(maxPath) == LoaderStatus.Loaded)
            {
                //max资源已经加载完毕，且没有必要再加载default和min资源，直接使用max资源
                priority = 0;
                _LoaderDatas.Add(_LoaderDataPool.Borrow().Init(loaderWrapper,maxPath, priority,callBack));
                return;
            }

            var minPath = Utils.GenerateAssetPath(loaderWrapper.Path, "1");
            if (!string.IsNullOrEmpty(minPath) && _AssetManager.GetAssetLoadStatus(minPath) == LoaderStatus.Loaded)
            {
                //min资源已经加载的资源，无需加载default资源,首先直接使用min资源，但还需要加载max资源
                priority = 0;
                _LoaderDatas.Add(_LoaderDataPool.Borrow().Init(loaderWrapper,minPath, priority,callBack));

                priority = loaderWrapper.Priority + 2;
                _LoaderDatas.Add(_LoaderDataPool.Borrow().Init(loaderWrapper,maxPath, priority,callBack));
                return;
            }

            var defaultPath = loaderWrapper.Path;
            
            if (!_Path2AssetPath.ContainsKey(loaderWrapper.Path))
            {
                _Path2AssetPath.Add(loaderWrapper.Path, new AssetPath()
                {
                    DefaultPath = defaultPath,
                    MinPath = minPath,
                    MaxPath = maxPath,
                    DefaultAssetStatus = AssetStatus.None,
                    MinAssetStatus = AssetStatus.None,
                    MaxAssetStatus = AssetStatus.None,
                });
            }
            
            //default资源还未加载，需要等待default资源加载完毕，再加载min资源
            if (!string.IsNullOrEmpty(defaultPath))
            {
                priority = loaderWrapper.Priority;
                _LoaderDatas.Add(_LoaderDataPool.Borrow().Init(loaderWrapper,defaultPath, priority,callBack));
                _Path2AssetPath[loaderWrapper.Path].DefaultAssetStatus = AssetStatus.Loading;
            }
            
            //min和资源还未加载，需要等待min资源加载完毕，再加载max资源
            if (!string.IsNullOrEmpty(minPath))
            {
                priority = loaderWrapper.Priority + 1;
                _LoaderDatas.Add(_LoaderDataPool.Borrow().Init(loaderWrapper,minPath, priority,callBack));
                _Path2AssetPath[loaderWrapper.Path].MinAssetStatus = AssetStatus.Loading;
            }

            //max资源还未加载，需要等待max资源加载完毕
            if (!string.IsNullOrEmpty(maxPath))
            {
                priority = loaderWrapper.Priority + 2;
                _LoaderDatas.Add(_LoaderDataPool.Borrow().Init(loaderWrapper,maxPath, priority,callBack));
                _Path2AssetPath[loaderWrapper.Path].MaxAssetStatus = AssetStatus.Loading;
            }
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