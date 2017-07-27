using System.Collections.Generic;
using UnityEngine;
using GB.EventSystems;

namespace GB.Character.RPG
{	
	public class GB_DMG : GB_RPGAttribute
	{
		[SerializeField] protected string allyTag;
		[SerializeField] protected Transform offender;
		[SerializeField] protected GameObject defaultPrefab;
		[SerializeField] protected string effectType = "Physical";

		[Header("Events")]
        [SerializeField] protected GB_NamedFloatEvent emitContact;
		[SerializeField] protected GB_NamedFloatEvent emitHit;

		public ParticleSystem ps { get; protected set; }
		public Vector3 hitPoint { get; protected set; }
		public Vector3 hitNormal { get; protected set; }

		protected readonly List<ParticleCollisionEvent> psCollisions = new List<ParticleCollisionEvent>();

		protected override void ExtendedStart()
		{
			if(offender == null) offender = transform;
			if(allyTag == null) allyTag = tag;
			if(ps == null) ps = GetComponent<ParticleSystem>();
		}

		protected virtual void OnContact(GameObject other)
		{
			if (!isActiveAndEnabled) return;
			if (other.tag == allyTag) return;

			GameObject prefab = null;
			float effect = 0;
			GB_HP hp = other.GetComponent<GB_HP>();
			if (hp)
			{
				effect = curr * Mathf.Max(Mathf.Abs(hp.TakeImpact(transform.position)), 1);
				hp.TakeDemage(effectType, effect);
				hp.SetOffender(offender);
				if (!hp.Block) prefab = hp.Prefab;
				emitHit.Invoke(effectType, effect);
			}

			if (prefab)
			{
				Instantiate(prefab, hitPoint, Quaternion.LookRotation(hitNormal, transform.up), other.transform);
			}
			else if (defaultPrefab)
			{
				Instantiate(defaultPrefab, hitPoint, Quaternion.LookRotation(hitNormal, transform.up));
			}
			emitContact.Invoke(effectType, effect);
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
			if(ps.GetCollisionEvents(other, psCollisions) > 0)
			{
				hitPoint = psCollisions[0].intersection;
				hitNormal = psCollisions[0].normal;
			}

			OnContact(other.gameObject);
		}
	}
}

