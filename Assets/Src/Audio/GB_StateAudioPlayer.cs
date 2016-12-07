using UnityEngine;

namespace GB.Audio
{
	public class GB_StateAudioPlayer : StateMachineBehaviour 
	{
		public enum State { Enter, Update, Exit }

        [SerializeField] AudioClip sound;
		[SerializeField] State playOn = State.Update;
        [SerializeField][Range (0f, 1f)] float cyrcleOffset = 0;
        [SerializeField][Range (0f, 2f)] float volume = 1;

        public AudioSource source { get; private set; }
        bool played;

        bool Init(Animator animator)
        {
			if (source == null)
			{
				source = animator.GetComponent<AudioSource>();
				/*
#if UNITY_EDITOR
				if (source == null)
					Debug.LogWarning("AudioSource missing! " + animator);
#endif
			*/
			}
			return source != null;
        }

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
			if (Init(animator) && playOn == State.Enter)
            {
                source.PlayOneShot(sound, volume);
				played = true;
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Init(animator) && playOn == State.Update)
            {
                if(!played && stateInfo.normalizedTime >= cyrcleOffset)
                {
                    source.PlayOneShot(sound, volume);
                    played = true;
                }
                else if(stateInfo.normalizedTime < cyrcleOffset)
                {
                    played = false;
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
			if (Init(animator) && playOn == State.Exit)
            {
                source.PlayOneShot(sound, volume);
            }
            played = false;
        }
    }
}
