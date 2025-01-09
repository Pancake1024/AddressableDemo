using System;
using Party;
using UnityEngine;

public class PreLoadShaders : MonoBehaviour
{
        public GameObject[] Objs;
        
        private void Start()
        {
                AssetManager.Instance.LoadShaderVariants("Assets/Raw/SVC.shadervariants", (shadervariants) =>
                {
                        shadervariants.WarmUp();
                        foreach (var obj in Objs)
                        { 
                                obj.SetActive(true);
                        }
                });
        }
}