using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Party
{
    public class AssetLoaderManager : MonoBehaviour
    {
        [SerializeField]
        [Header("同时加载的AssetLoader数量")]
        private int PARALLEL_MAX_LOADERS_COUNT = 10;

        [SerializeField]
        [Header("更新AssetLoader列表的间隔时间")]
        private float _UpdateLoaderTime = 0.1f;
        private float _Time;
        
        private List<IAssetLoader> _AssetLoaders;

        private bool _CanUpdate = false;
        
        void Awake()
        {
            _AssetLoaders = new List<IAssetLoader>(PARALLEL_MAX_LOADERS_COUNT);
        }
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                AssetManager.Instance.ReleaseAll();
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _CanUpdate = !_CanUpdate;
            }

            if (!_CanUpdate) return;

            if (_Time > _UpdateLoaderTime)
            {
                _Time = 0;
            }
            else
            {
                _Time += Time.deltaTime;
                return;
            }
            
            int count = PARALLEL_MAX_LOADERS_COUNT - _AssetLoaders.Count;
            count = count > AssetLoaderHelper.PendingAssetLoader.Count ? AssetLoaderHelper.PendingAssetLoader.Count : count;
            for (int i = 0; i < count; i++)
            {
                var loader = AssetLoaderHelper.PendingAssetLoader[0];
                AssetLoaderHelper.PendingAssetLoader.RemoveAt(0);
                _AssetLoaders.Add(loader);
            }
            
            for (int i = 0; i < _AssetLoaders.Count; i++)
            {
                var loader = _AssetLoaders[i];
                if (loader.Update())
                {
                    // Debug.LogError($"loader done:{loader.GetType().Name} {loader.Priority}");
                    _AssetLoaders.RemoveAt(i);
                    //TODO:Pool
                    loader.Release();
                    i--;
                }
            }

        }
    }
}