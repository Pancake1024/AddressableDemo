using System;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Party
{
    public interface IAssetManager
    {
        void InstantiateAsync(string path, Action<GameObject> callback, Transform parent = null,
            bool worldPositionStays = true);

        void InstantiateAsync(string path, Action<GameObject> callback, Vector3 position,
            Quaternion rotation, Transform parent = null);

        void LoadSpriteAsync(string path, Action<Sprite> callback);
        
        void LoadTexture2DAsync(string path, Action<Texture2D> callback);
        
        void LoadMaterialAsync(string path, Action<Material> callback);
        
        void LoadMeshAsync(string path, Action<Mesh> callback);

        void LoadShaderVariants(string path, Action<ShaderVariantCollection> callback);

        void LoadScene(string path, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true,
            int priority = 100, SceneReleaseMode releaseMode = SceneReleaseMode.ReleaseSceneWhenSceneUnloaded);
        
        void ReleaseAsset(string path);
        
        void ReleaseAll();

        void GetDownloadSizeAsync(string path, Action<long> callback);
        
        LoaderStatus GetAssetLoadStatus(string path);

        void AddPersistentAsset(string path);
        
        void AddPersistentAssets(string[] paths);
        
        void RemovePersistentAsset(string path);
        
        void RemovePersistentAssets(string[] paths);
    }
}