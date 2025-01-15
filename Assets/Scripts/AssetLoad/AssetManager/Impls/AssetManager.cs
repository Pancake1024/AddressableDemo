using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Party
{
    /// <summary>
    /// 使用Addressables对IAssetManager接口的实现
    /// </summary>
    public class AssetManager : IAssetManager
    {
        private const int INIT_PERSISTENT_ASSET_COUNT = 32;
        private const int INIT_CAPACITY = 128;
        
        private Dictionary<string,IAssetWrapper> _Path2AssetWrapper = new Dictionary<string, IAssetWrapper>(INIT_CAPACITY);
        private List<string> _PersistentAssets = new List<string>(INIT_PERSISTENT_ASSET_COUNT);
        
        private void _LoadAssetAsync<T>(string path, Action<T> callback) where T : Object
        {
            if (_Path2AssetWrapper.TryGetValue(path, out IAssetWrapper assetWrapper))
            {
                assetWrapper.LoadAssetAsync(path, callback);
                return;
            }
            
            var newAssetWrapper = new AssetWrapper();
            _Path2AssetWrapper.Add(path, newAssetWrapper);
            newAssetWrapper.LoadAssetAsync(path, callback);
        }
        
        public void InstantiateAsync(string path, Action<GameObject> callback,Transform parent = null,bool worldPositionStays = true)
        {
            _LoadAssetAsync<GameObject>(path, obj =>
            {
                if (obj != null)
                {
                    var go = GameObject.Instantiate(obj, parent, worldPositionStays);
                    callback?.Invoke(go);
                }
                else
                {
                    callback?.Invoke(null);
                }
            });
        }

        public void InstantiateAsync(string path, Action<GameObject> callback, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            _LoadAssetAsync<GameObject>(path, obj =>
            {
                if (obj != null)
                {
                    var go = GameObject.Instantiate(obj, position, rotation, parent);
                    callback?.Invoke(go);
                }
                else
                {
                    callback?.Invoke(null);
                }
            });
        }
        
        public void LoadSpriteAsync(string path, Action<Sprite> callback)
        {
            _LoadAssetAsync<Sprite>(path, callback);
        }
        
        public void LoadTexture2DAsync(string path, Action<Texture2D> callback)
        {
            _LoadAssetAsync<Texture2D>(path, callback);
        }
        
        public void LoadMaterialAsync(string path, Action<Material> callback)
        {
            _LoadAssetAsync<Material>(path, callback);
        }

        public void LoadMeshAsync(string path, Action<Mesh> callback)
        {
            _LoadAssetAsync<Mesh>(path, callback);
        }

        public void LoadShaderVariants(string path, Action<ShaderVariantCollection> callback)
        {
            _LoadAssetAsync<ShaderVariantCollection>(path, callback);
        }

        public void LoadScene(string path, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true,
            int priority = 100, SceneReleaseMode releaseMode = SceneReleaseMode.ReleaseSceneWhenSceneUnloaded)
        {
            Addressables.LoadSceneAsync(path, loadMode, activateOnLoad, priority, releaseMode);
        }

        public void ReleaseAsset(string path)
        {
            Debug.LogError($"try release asset {path}");
            if (_PersistentAssets.Contains(path))
            {
                return;
            }
            
            if (_Path2AssetWrapper.TryGetValue(path, out IAssetWrapper assetWrapper))
            {
                assetWrapper.Release();
                _Path2AssetWrapper.Remove(path);
            }
        }

        public void ReleaseAll()
        {
            foreach (var kv in _Path2AssetWrapper)
            {
                if (_Path2AssetWrapper.ContainsKey(kv.Key))
                {
                    continue;
                }
                kv.Value.Release();
            }
        }

        public void GetDownloadSizeAsync(string path, Action<long> callback)
        {
            Addressables.GetDownloadSizeAsync(path).Completed += opHandle =>
            {
                callback?.Invoke(opHandle.Result);
                opHandle.Release();
            };
        }

        public LoaderStatus GetAssetLoadStatus(string path)
        {
            if (_Path2AssetWrapper.TryGetValue(path, out IAssetWrapper assetWrapper))
            {
                return assetWrapper.GetLoaderStatus();
            }
            
            return LoaderStatus.None;
        }

        public void AddPersistentAsset(string path)
        {
            _PersistentAssets.Add(path);
        }

        public void AddPersistentAssets(string[] paths)
        {
            _PersistentAssets.AddRange(paths);
        }

        public void RemovePersistentAsset(string path)
        {
            _PersistentAssets.Remove(path);
        }

        public void RemovePersistentAssets(string[] paths)
        {
            foreach (var path in paths)
            {
                _PersistentAssets.Remove(path);
            }
        }
    }
}