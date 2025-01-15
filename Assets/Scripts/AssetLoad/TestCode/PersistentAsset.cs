using System;
using UnityEngine;

namespace Party
{
    public class PersistentAsset : MonoBehaviour
    {
        public string[] _Addresses;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AssetLoaderManager.Instance.AddPersistentAssets(_Addresses);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                AssetLoaderManager.Instance.RemovePersistentAssets(_Addresses);
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                AssetLoaderManager.Instance.ReleaseAll();
            }
        }
    }
}