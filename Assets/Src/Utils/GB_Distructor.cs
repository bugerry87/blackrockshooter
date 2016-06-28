using UnityEngine;

namespace GBAssets.Utils
{
    public class GB_Distructor : MonoBehaviour
	{
		[SerializeField] protected float delay;

		void Start()
		{
			Destroy(gameObject, delay);
		}
	}
}
