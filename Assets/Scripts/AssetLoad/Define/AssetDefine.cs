using System;
using Object = UnityEngine.Object;

namespace Party
{
    public enum AssetLoadStatus
    {
        None,
        Loading,
        Loaded,
        Failed,
    }
    
    public class LoaderData
    {
        public ILoaderWrapper LoaderWrapper;
        public string Path;
        public int Priority;
        public Action<Object> CallBack;
            
        public LoaderData Init(ILoaderWrapper loaderWrapper, string path, int priority, Action<Object> callBack)
        {
            LoaderWrapper = loaderWrapper;
            Path = path;
            Priority = priority;
            CallBack = callBack;
            return this;
        }
            
        public void Release()
        {
            LoaderWrapper = null;
            Path = null;
            Priority = 0;
            CallBack = null;
        }
    }

}