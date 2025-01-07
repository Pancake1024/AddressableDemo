using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Party
{
    [ExecuteAlways]
    public abstract class AbstractLoaderWrapper : MonoBehaviour,ILoaderWrapper
    {
        [SerializeField]
        protected string _Path;
        [SerializeField]
        protected int _Priority = 3;
        
        private bool _IsInit = false;
        
        public string Path => _Path;
        public int Priority => _Priority;

        public void SetPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (path.Equals(_Path))
            {
                return;
            }
            
            _Path = path;
            LoadAssetAsync(path);
        }

        private void Awake()
        {
            OnAwake();
        }

        private void OnEnable()
        {
            if (!Application.isPlaying) return;
            if (_IsInit)return;
            _IsInit = true;
            if (!string.IsNullOrEmpty(_Path))
            {
                LoadAssetAsync(_Path);
            }
        }

        protected abstract void OnAwake();
        protected abstract void LoadAssetAsync(string path);
    }
}