using System;
using UnityEngine;

namespace Party
{
    public class GameObjectLoaderWrapper : AbstractLoaderWrapper
    {
        public Transform Parent;
        public Action<GameObject> OnLoaded;
        
        private GameObject _LoadedGameObject;
        
        protected override void OnAwake()
        {
            Parent = transform;    
        }

        protected override void LoadAssetAsync(string path)
        {
            AssetLoaderManager.Instance.AddLoaderWrapper(this, obj =>
            {
                if (_IsDestory)
                {
                    Destroy(obj);
                    OnLoaded?.Invoke(null);
                    return;
                }
                
                if (_LoadedGameObject != null)
                {
                    Destroy(_LoadedGameObject);
                    _LoadedGameObject = null;
                }
                
                var go = obj as GameObject;
                go.transform.SetParent(Parent);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                
                _LoadedGameObject = go;
                
                OnLoaded?.Invoke(go);
                OnLoaded = null;
            });
        }
    }
}