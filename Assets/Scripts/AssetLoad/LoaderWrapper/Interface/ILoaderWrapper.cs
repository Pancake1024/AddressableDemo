using System;

namespace Party
{
    public interface ILoaderWrapper
    {
        string Path { get; }
        
        int Priority { get; }
        
        void SetPath(string path);
        
    }
}