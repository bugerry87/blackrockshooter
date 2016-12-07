using UnityEngine;

namespace GB.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class GB_AnimationSound : MonoBehaviour
	{
		[SerializeField][Range (0f, 1f)] float randomize = 0;

		public AudioSource Source { get; private set; }

		void Start()
		{
			Source = GetComponent<AudioSource>();
		}

		public void PlaySound(Object clip)
		{
			if (enabled) PlaySound((AudioClip) clip, 1);
		}

		public void PlaySound(AudioClip clip, float volume = 1)
		{
			if (enabled)
			{
				volume = Mathf.Clamp(volume, 0, 1);
				Source.pitch = 1 - Random.value * randomize;
				Source.PlayOneShot(clip, volume - Random.value * randomize);
			}
		}
	}
}

