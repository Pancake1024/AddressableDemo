using System;
using UnityEngine;

namespace Party
{
    public class PreLoadShaderVariants : MonoBehaviour
    {
        public GameObject[] Objs;
        private void Start()
        {
            AssetLoaderManager.Instance.LoadShaderVariants("Assets/Raw/SVC.shadervariants", () =>
            {
                foreach (var obj in Objs)
                {
                    obj.SetActive(true);
                }
            });
        }
    }
}