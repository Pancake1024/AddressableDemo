using System;
using UnityEngine;
using UnityEngine.UI;

namespace Party
{
    public class PreLoadShaderVariants : MonoBehaviour
    {
        public GameObject[] Objs;

        private void Start()
        {
            AssetLoaderManager.Instance.LoadShaderVariants("Assets/Raw/SVC.shadervariants", svc =>
            {
                svc.WarmUp();
                foreach (var obj in Objs)
                {
                    obj.SetActive(true);
                }
            });
        }
    }
}