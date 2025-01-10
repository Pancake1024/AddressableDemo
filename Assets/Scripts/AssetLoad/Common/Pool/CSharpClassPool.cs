using System;
using IO.Unity3D.Source.Pool;

namespace Party
{
    public class CSharpClassPool<T> : BasePool<T>
    {
        protected CSharpClassPool(int initSize, int maxSize, Func<T> creator, Action<T> onBorrow, Action<T> onReturn, Action<T> onDestroy) : base(initSize, maxSize, creator, onBorrow, onReturn, onDestroy)
        {
        }

        public static CSharpClassPool<T> Build(int initSize, int maxSize, Func<T> creator, Action<T> onBorrow,
            Action<T> onReturn, Action<T> onDestroy)
        {
            var pool = new CSharpClassPool<T>(initSize, maxSize, creator, onBorrow, onReturn, onDestroy);
            pool.Init();
            return pool;
        }
    }
}