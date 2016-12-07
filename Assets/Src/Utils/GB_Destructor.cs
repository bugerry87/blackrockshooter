using UnityEngine;

namespace GB.Utils
{
    public class GB_Destructor : MonoBehaviour
	{
		[SerializeField] protected float delay;

		void Start()
		{
			Destroy(gameObject, delay);
		}
	}
}
