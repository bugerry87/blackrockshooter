using UnityEngine;

namespace GB.Utils
{
    public class GB_Destructor : MonoBehaviour
	{
		[SerializeField] protected float delay;

		void Start()
		{
			Destroy(delay);
		}

		public void Destroy(float delay)
		{
			Destroy(gameObject, delay);
		}
	}
}
