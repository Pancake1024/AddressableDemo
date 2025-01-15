using System;
using UnityEngine;

namespace Party
{
    public class PersistentAsset : MonoBehaviour
    {
        public string[] _Addresses;

        private void Start()
        {
            AssetLoaderManager.Instance.AddPersistentAssets(_Addresses);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                AssetLoaderManager.Instance.ReleaseAll();
            }
        }
    }
}