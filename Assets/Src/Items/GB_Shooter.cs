using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace GBAssets.Items
{
	[RequireComponent(typeof(Animator))]
	public class GB_Shooter : MonoBehaviour
	{
		[SerializeField]
		GameObject prefab = null;

		[SerializeField]
		float spread = 0;

		[SerializeField]
		string button = "Fire1";

		[SerializeField]
		string stateName = "Shoot";

		[SerializeField]
		Vector3 offset = Vector3.zero;

		protected Animator animator {get; private set;}

		void Start()
		{
			animator = GetComponent<Animator>();
		}

		void FixedUpdate()
		{
			if (CrossPlatformInputManager.GetButton(button))
			{
				animator.SetBool(stateName, true);
			}
			else
			{
				animator.SetBool(stateName, false);
			}
		}

		public void ApplyShoot()
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
