using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Pool
{
    public class PoolManager : MonoBehaviour
    {
        private static Dictionary<string, Pool> pools;

        void Awake()
        {
            pools = new Dictionary<string, Pool>();
        }

        void OnDestroy()
        {
            pools = null;
        }

        public static T GetObject<T>(string path, PlaceholderFactory<string, T> factory) where T : PoolObject
        {
            if (!pools.TryGetValue(path, out var pool))
            {
                pool = new Pool(path);
                pools.Add(path, pool);
            }

            var poolObj = pool.GetObject(factory);

            return poolObj;
        }

        public static void ReturnObject(PoolObject obj, string path)
        {
            if (!pools.TryGetValue(path, out var pool))
            {
                Debug.LogErrorFormat("PoolManager: there is no pool at given path {0}", path);
                return;
            }

            pool.ReturnObject(obj);
        }

        public static void PreWarm<T>(string path, int objectCount, PlaceholderFactory<string, T> factory) where T : PoolObject
        {
            if (pools.ContainsKey(path))
            {
                return;
            }

            var objects = new List<T>(objectCount);

            for (int i = 0; i < objectCount; i++)
            {
                objects.Add(GetObject(path, factory));
            }

            for (int i = 0; i < objectCount; i++)
            {
                objects[i].OnPreWarmed();
                objects[i].ReturnObject();
            }
        }

        public static void ClearAllPools()
        {
            pools.Clear();
        }
    }
}
