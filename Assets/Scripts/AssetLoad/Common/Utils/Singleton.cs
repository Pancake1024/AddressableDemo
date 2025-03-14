﻿namespace Party
{
    public class Singleton<T> where T : class, new()
    {
        private static readonly object _lock = new object();
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }

        // Protected constructor to prevent instantiation outside the class
        protected Singleton() { }
    }

}