using UnityEngine;

//obsolete!!!
namespace GBAssets.Items
{
	[RequireComponent(typeof(Rigidbody))]
	public class GB_Bullet : MonoBehaviour
	{
		[SerializeField] string tagFilter = "Enemy";
		[SerializeField] float lifetime = 10;
		[SerializeField] float force = 10;
		[SerializeField] float demage = 10;

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
				Destroy(gameObject);
			}
			alive += Time.fixedDeltaTime;
		}

		void OnTriggerEnter(Collider other)
		{
			if (!other.isTrigger && other.tag == tagFilter)
			{
#if UNITY_EDITOR
				Debug.Log("HIT: " + demage);
#endif
				Destroy(gameObject);
			}
		}
	}
}

