using UnityEngine;

namespace VFX
{
    public class AnimationEffect : Effect
    {
        private Animation anim;

        public override bool IsPlaying
        {
            get { return anim.isPlaying; }
        }

        public override float Duration
        {
            get { return anim.clip.length; }
        }

        public override void SetSize()
        {
            //todo: доделать
        }

        public override void Initialize()
        {
            //todo: доделать
        }

        void Awake()
        {
            anim = GetComponent<Animation>();
            anim.wrapMode = WrapMode.Once;

            if (anim == null)
            {
                Debug.LogError("there is no Animation component attached");
            }
        }

        public override void Play()
        {
            anim.Play();
        }

        public override void Stop()
        {
            anim.Stop();
        }
    }
}
