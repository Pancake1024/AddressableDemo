using UnityEngine;

namespace Party
{
    public abstract class AbstractAssetLoader : IAssetLoader
    {
        protected bool _IsDone;
        protected int _Priority;
        
        protected string _Path;
        protected IAssetManager _AssetManager;
        protected System.Action<Object> _Callback;

        private bool _IsInit;

        public int Priority => _Priority;

        public IAssetLoader InitLoader(string path, int priority,IAssetManager assetManager, System.Action<Object> callback)
        {
            _Path = path;
            _Priority = priority;
            _AssetManager = assetManager;
            _Callback = callback;
            return this;
        }

        public void Release()
        {
            _Path = null;
            _AssetManager = null;
            _Callback = null;
            _IsDone = false;
            _IsInit = false;
        }

        public bool Update()
        {
            if (!_IsInit)
            {
                _IsInit = true;
                OnStartLoad();
            }

            return _IsDone;
        }

        public void StartLoad()
        {
            OnStartLoad();
        }
        
        protected abstract void OnStartLoad();
    }
}