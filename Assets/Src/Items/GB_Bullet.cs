using UnityEngine;
using System.Collections;

namespace GBAssets.Items
{
	[RequireComponent(typeof(Rigidbody))]
	public class GB_Bullet : MonoBehaviour
	{
		[SerializeField]
		float lifetime = 10;
		[SerializeField]
		float force = 10;
		[SerializeField]
		float demage = 10;

		protected Rigidbody rig {get; private set;}
		protected float alive {get; private set;}

		void Start()
		{
			rig = GetComponent<Rigidbody>();
			rig.AddRelativeForce(0, 0, force, ForceMode.VelocityChange);
		}

		void FixedUpdate()
		{
			transform.forward += rig.velocity;
			
			if (alive > lifetime)
			{
				GameObject.Destroy(gameObject);
			}
			alive += Time.fixedDeltaTime;
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.tag.Equals("Player"))
			{
#if UNITY_EDITOR
				Debug.Log("HIT");
#endif
				GameObject.Destroy(gameObject);
			}
		}
	}
}

