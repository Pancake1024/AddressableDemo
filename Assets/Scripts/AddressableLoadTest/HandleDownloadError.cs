using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;

namespace Test
{
    public class HandleDownloadError : MonoBehaviour
    {
        private string _Address;
        private AsyncOperationHandle _Handle;

        void LoadAsset()
        {
            _Handle = Addressables.LoadSceneAsync(_Address);
            _Handle.Completed += handle =>
            {
                string error = _GetDownloadError(_Handle);
                if (!string.IsNullOrEmpty(error))
                {
                    //TODO:
                }
            };
        }

        string _GetDownloadError(AsyncOperationHandle handle)
        {
            if (handle.Status != AsyncOperationStatus.Failed)
            {
                return null;
            }

            RemoteProviderException remoteException;
            System.Exception e = handle.OperationException;
            while (e != null)
            {
                remoteException = e as RemoteProviderException;
                if (remoteException != null)
                {
                    return remoteException.WebRequestResult.Error;
                }
                
                e = e.InnerException;
            }

            return null;
        }
    }
}