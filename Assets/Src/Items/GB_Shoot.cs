using UnityEngine;
using System.Collections;

namespace GBAssets.Items
{
	public class GB_Shoot : MonoBehaviour
	{
		[SerializeField]
		GameObject prefab = null;

		[SerializeField]
		float spread = 0;

		[SerializeField]
		Vector3 offset = Vector3.zero;

		void ApplyShoot()
		{
			if (prefab != null)
			{
				if (spread == 0)
				{
					Instantiate(prefab, transform.position + transform.TransformVector(offset), transform.rotation);
				}
				else
				{
					Vector3 random = transform.forward + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
					Instantiate(prefab, transform.position + transform.TransformVector(offset), Quaternion.LookRotation(random));
				}
			}
		}
	}
}
