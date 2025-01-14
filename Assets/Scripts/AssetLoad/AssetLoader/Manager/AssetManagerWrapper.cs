using System;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Party
{
    /// <summary>
    /// 对AssetManager的接口进行封装，提供给外部调用
    /// </summary>
    public partial class AssetLoaderManager : IAssetManager
    {
        public void InstantiateAsync(string path, Action<GameObject> callback, Transform parent = null, bool worldPositionStays = true)
        {
            _AssetManager.InstantiateAsync(path, callback, parent, worldPositionStays);
        }

        public void InstantiateAsync(string path, Action<GameObject> callback, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            _AssetManager.InstantiateAsync(path, callback, position, rotation, parent);
        }

        public void LoadSpriteAsync(string path, Action<Sprite> callback)
        {
            _AssetManager.LoadSpriteAsync(path, callback);
        }

        public void LoadTexture2DAsync(string path, Action<Texture2D> callback)
        {
            _AssetManager.LoadTexture2DAsync(path, callback);
        }

        public void LoadMaterialAsync(string path, Action<Material> callback)
        {
            _AssetManager.LoadMaterialAsync(path, callback);
        }

        public void LoadMeshAsync(string path, Action<Mesh> callback)
        {
            _AssetManager.LoadMeshAsync(path, callback);
        }

        public void LoadShaderVariants(string path, Action<ShaderVariantCollection> callback)
        {
            _AssetManager.LoadShaderVariants(path, callback);
        }

        public void LoadScene(string path, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true,
            int priority = 100, SceneReleaseMode releaseMode = SceneReleaseMode.ReleaseSceneWhenSceneUnloaded)
        {
            _AssetManager.LoadScene(path, loadMode, activateOnLoad, priority, releaseMode);
        }

        public void ReleaseAsset(string path)
        {
            _AssetManager.ReleaseAsset(path);
        }

        public void ReleaseAll()
        {
            _WrapperLoaderCacheManager.PendingAssetLoader.Clear();
            
            _AssetManager.ReleaseAll();
        }

        public void GetDownloadSizeAsync(string path, Action<long> callback)
        {
            _AssetManager.GetDownloadSizeAsync(path, callback);
        }

        public LoaderStatus GetAssetLoadStatus(string path)
        {
            return _AssetManager.GetAssetLoadStatus(path);
        }

        public void AddPersistentAsset(string path)
        {
            _AssetManager.AddPersistentAsset(path);
        }

        public void AddPersistentAssets(string[] paths)
        {
            _AssetManager.AddPersistentAssets(paths);
        }

        public void RemovePersistentAsset(string path)
        {
            _AssetManager.RemovePersistentAsset(path);
        }

        public void RemovePersistentAssets(string[] paths)
        {
            _AssetManager.RemovePersistentAssets(paths);
        }
    }
}