using UnityEngine;

namespace GB.Character.RPG
{
	public class GB_RPGCollectable : MonoBehaviour
	{
		[SerializeField] string collectorTag;
		[SerializeField] string message;
		[SerializeField] float value;

		void OnTriggerEnter(Collider other)
		{
			if (
				message.Length > 0 &&
				collectorTag.Length == 0 || 
				collectorTag.Contains(other.tag)
			) {
				other.SendMessage(message, value);
				Destroy(gameObject);
			}
		}
	}
}

