using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Party
{
    public class AssetManager : Singleton<AssetManager>, IAssetManager
    {
        private Dictionary<string, AsyncOperationHandle> _Path2Handle = new Dictionary<string, AsyncOperationHandle>();
        private Dictionary<string, List<Action<object>>> _Path2PendingCallbacks = new Dictionary<string, List<Action<object>>>();
        
        private void _LoadAssetAsync<T>(string path, Action<T> callback) where T : Object
        {
            if (_Path2Handle.ContainsKey(path))
            {
                var opHandle = _Path2Handle[path];
                if (opHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(opHandle.Result as T);
                }
                else
                {
                    if (!_Path2PendingCallbacks.ContainsKey(path))
                    {
                        _Path2PendingCallbacks[path] = new List<Action<object>>();
                    }
                    _Path2PendingCallbacks[path].Add(obj =>
                    {
                        callback?.Invoke(obj as T);
                    });
                }
                return;
            }
            
            var newOpHandle = Addressables.LoadAssetAsync<T>(path);
            _Path2Handle.Add(path, newOpHandle);
            newOpHandle.Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(handle.Result);

                    if (_Path2PendingCallbacks.ContainsKey(path))
                    {
                        foreach (var cb in _Path2PendingCallbacks[path])
                        {
                            cb?.Invoke(handle.Result);
                        }
                        _Path2PendingCallbacks.Remove(path);
                    }
                }
                else
                {
                    Debug.LogError($"Load {path} failed");
                    callback?.Invoke(default);
                    if (_Path2PendingCallbacks.ContainsKey(path))
                    {
                        foreach (var cb in _Path2PendingCallbacks[path])
                        {
                            cb?.Invoke(default);
                        }
                        _Path2PendingCallbacks.Remove(path);
                    }
                }
            };
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

        public void ReleaseAsset(string path)
        {
            if (_Path2Handle.ContainsKey(path))
            {
                _Path2Handle[path].Release();
                _Path2Handle.Remove(path);
            }
        }

        public void ReleaseAll()
        {
            foreach (var kv in _Path2Handle)
            {
                kv.Value.Release();
            }

            _Path2Handle.Clear();
            _Path2PendingCallbacks.Clear();
        }
    }
}