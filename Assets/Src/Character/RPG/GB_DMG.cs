using UnityEngine;
using UnityEngine.Events;

namespace GBAssets.Character.RPG
{	
	public class GB_DMG : GB_RPGAttribute
	{
		[SerializeField] protected string allyTag;
		[SerializeField] protected Transform offender;
		[SerializeField] protected GameObject defaultPrefab;
        [SerializeField] protected UnityEvent emitContact;

		public ParticleSystem ps { get; protected set; }
		public Vector3 hitPoint { get; protected set; }
		public Vector3 hitNormal { get; protected set; }

		protected readonly ParticleCollisionEvent[] psCollisions = new ParticleCollisionEvent[1];

		protected override void ExtendedStart()
		{
			if(offender == null) offender = transform;
			if(allyTag == null) allyTag = tag;
		}

		protected virtual void OnContact(GameObject other)
		{
			GameObject prefab = null;
            emitContact.Invoke();
            if (other.tag != allyTag)
			{
				GB_HP hp = other.GetComponent<GB_HP>();
				if (hp)
				{
					hp.TakeDemage(curr * Mathf.Max(Mathf.Abs(hp.TakeImpact(transform.position)), 1));
					hp.SetOffender(offender);
					prefab = hp.Prefab;
#if UNITY_EDITOR
					Debug.Log("HitPoint: " + hitPoint + " HitNormal: " + hitNormal);
#endif
				}
			}

			if (prefab)
			{
				Instantiate(prefab, hitPoint, Quaternion.LookRotation(hitNormal, transform.up));
			}
		}

		void OnTriggerEnter(Collider other)
		{
			if (!other.isTrigger)
            {
				hitPoint = transform.position;
				hitNormal = -transform.forward;
                OnContact(other.gameObject);
            }
		}

		void OnParticleCollision(GameObject other)
		{
			if(ps == null)
			{
				ps = GetComponent<ParticleSystem>();
			}

			if(ps.GetCollisionEvents(other, psCollisions) > 0)
			{
				hitPoint = psCollisions[0].intersection;
				hitNormal = psCollisions[0].normal;
				if(defaultPrefab)
				{
					Instantiate(defaultPrefab, hitPoint, Quaternion.LookRotation(hitNormal, transform.up));
				}
			}

			OnContact(other.gameObject);
		}
	}
}

