using UnityEngine;
using UnityEngine.Events;
using GBAssets.EventSystems;

namespace GBAssets.Character.RPG
{	
	public class GB_HP : GB_RPGAttribute, GB_ILiveHandler
	{
		[SerializeField] protected string demageId = "Demage";
        [SerializeField] protected string impcatId = "Impact";
		[SerializeField] protected string HpId = "HP";
		[SerializeField] GameObject prefab;
		
		[SerializeField] protected GB_TransformEvent emitOffender;
		[SerializeField] protected GB_FloatEvent emitDemage;
        [SerializeField] protected GB_FloatEvent emitImpact;
		[SerializeField] protected GB_FloatEvent emitHp;
		[SerializeField] protected UnityEvent emitKO;

		public GameObject Prefab { get { return prefab; } private set { prefab = value; } }
		public float immortality { get; set; } //Quick n Dirty

		protected override void ExtendedStart()
		{
			immortality = 1;
		}

		public void Heal(float heal)
		{
			curr += heal;
			emitHp.Invoke(HpId, Percentage());
		}

		public void Recover()
		{
			curr = max;
		}

		public void TakeDemage(float demage)
		{
			demage *= immortality;
			curr -= demage;
			emitDemage.Invoke(demageId, demage);
			if(curr == 0)
			{
				emitKO.Invoke();
			}
#if UNITY_EDITOR
			Debug.Log("Demage: " + demage);
#endif
		}

        public float TakeImpact(Vector3 impact)
		{
			Vector3 dir = Vector3.ProjectOnPlane(transform.position - impact, Vector3.up).normalized;
            float forward = Vector3.Dot(transform.forward, dir) + 1;

            if (Vector3.Dot(transform.right, dir) < 0)
            {
                forward *= -1;
            }

			emitImpact.Invoke(impcatId, forward);
			return forward;
		}

		public void SetOffender(Transform offender)
		{
			emitOffender.Invoke(offender);
		}

		public float Percentage()
		{
			return curr / max;
		}

		public void Destroy(float time)
		{
			Destroy(gameObject, time);
		}
	}
}

