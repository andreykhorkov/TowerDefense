//using UnityEngine;

//namespace VFX
//{
//    public class SpriteAnimationEffect : Effect
//    {
//        private tk2dSpriteAnimator spriteAnimator;

//        public override bool IsPlaying
//        {
//            get { return spriteAnimator.IsPlaying(spriteAnimator.CurrentClip); }
//        }

//        public override float Duration
//        {
//            get { return spriteAnimator.CurrentClip.frames.Length/spriteAnimator.CurrentClip.fps; }
//        }

//        public override void SetSize()
//        {
//            spriteAnimator.Sprite.scale = Toy.ScaleFactor*Vector3.one;
//        }

//        public override void Initialize()
//        {
//            if (isInitialized)
//            {
//                return;
//            }

//            SetSize();

//            isInitialized = true;
//        }

//        void Awake()
//        {
//            spriteAnimator = GetComponent<tk2dSpriteAnimator>();

//            if (spriteAnimator == null)
//            {
//                Debug.LogError("there is no tk2dSpriteAnimator component attached");
//            }
//        }

//        public override void Play()
//        {
//            spriteAnimator.Play();
//        }

//        public override void Stop()
//        {
//            spriteAnimator.Stop();
//        }
//    }
//}
