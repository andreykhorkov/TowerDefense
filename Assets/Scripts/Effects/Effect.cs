using System.Collections;
using Pool;
using UnityEngine;

namespace VFX
{
    public abstract class Effect : PoolObject
    {
        protected bool isInitialized;

        public abstract void Play();
        public abstract void Stop();
        public abstract bool IsPlaying { get; }
        public abstract float Duration { get; }

        public abstract void SetSize();
        public abstract void Initialize();

        public override void OnTakenFromPool()
        {
            Initialize();
            StartCoroutine(Playing());
        }

        public override void OnReturnedToPool()
        {
            Stop();
            SetOrientation(Vector3.zero, Quaternion.identity);
        }

        public override void OnPreWarmed()
        {
            ReturnObject();
        }

        public IEnumerator Playing()
        {
            Play();

            while (IsPlaying)
            {
                yield return null;
            }

            ReturnObject();
        }
    }
}
