using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Test
{
    public class LoadScene : MonoBehaviour
    {
        private string _Addresss = "Assets/Raw/Scenes/AddressTestScene.unity";
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Addressables.LoadSceneAsync(_Addresss, LoadSceneMode.Additive,true,100);
            }
        }
    }
}