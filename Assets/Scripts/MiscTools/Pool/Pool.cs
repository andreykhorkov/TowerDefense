using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Pool
{
    public class Pool
    {
        private Queue<PoolObject> objects;
        private string path;
        private PoolObject asset;

        public Pool(string path)
        {
            this.path = path;
            asset = Resources.Load<PoolObject>(path);
            objects = new Queue<PoolObject>();
        }

        public T GetObject<T>(PlaceholderFactory<string, T> factory) where T : PoolObject
        {
            T obj;

            if (objects.Count > 0)
            {
                obj = (T) objects.Dequeue();
            }
            else
            {
                obj = (T) factory.Create(path);
                obj.SetPoolKey(path);
            }

            obj.OnTakenFromPool();

            return obj;
        }

        public void ReturnObject(PoolObject obj)
        {
            objects.Enqueue(obj);
            obj.OnReturnedToPool();
        }
    }
}
