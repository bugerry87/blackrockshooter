using UnityEngine;
using UnityEngine.SceneManagement;

namespace GB.UI
{
	public class GB_GameMenu : MonoBehaviour
	{
		static GB_GameMenu master = null;
		static float startTimeScale = 1;
		static float goalTimeScale = 1;
		static float fadeTime = 0;
		static float fadeBase = 0;
		
		void Update()
		{
			if (master == this)
			{
				if (fadeBase != 0)
				{
					fadeTime -= Time.deltaTime;
					Time.timeScale = Mathf.Lerp(goalTimeScale, startTimeScale, Mathf.Max(0, fadeTime / fadeBase));
				}
				else
				{
					Time.timeScale = goalTimeScale;
				}
			}
		}

		public void Pause(float fade_time = 0)
		{
			startTimeScale = Time.timeScale;
			goalTimeScale = 0;
			fadeTime = fade_time;
			fadeBase = fade_time;
			master = this;
		}

		public void Unpause(float fade_time = 0)
		{
			startTimeScale = Time.timeScale;
			goalTimeScale = 1;
			fadeTime = fade_time;
			fadeBase = fade_time;
			master = this;
		}

		public void Restart()
		{
			master = this;
			startTimeScale = 1;
			goalTimeScale = 1;
			fadeTime = 0;
			fadeBase = 0;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}