using UnityEngine;
using Zenject;

namespace VFX
{
    public class ParticleEffect : Effect
    {
        private ParticleSystem particleEffect;

        public override bool IsPlaying
        {
            get { return particleEffect.isPlaying; }
        }

        public override float Duration
        {
            get { return particleEffect.main.duration; }
        }

        public override void SetSize()
        {
            var main = particleEffect.main;
            var mainCurve = particleEffect.main.startSize;
            mainCurve.constantMax = mainCurve.constantMax;
            mainCurve.constantMin = mainCurve.constantMin;
            main.startSize = mainCurve;
        }

        public override void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            SetSize();

            isInitialized = true;
        }

        void Awake()
        {
            particleEffect = GetComponent<ParticleSystem>();

            if (particleEffect == null)
            {
                Debug.LogError("there is no ParticleSystem component attached");
            }
        }

        public override void Play()
        {
            particleEffect.Play();
            StartCoroutine(Playing());
        }

        public override void Stop()
        {
            particleEffect.Stop();
        }

        public class Factory : PlaceholderFactory<string, ParticleEffect> { }
    }
}
