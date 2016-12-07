﻿using System;
using UnityEngine;
using UnityEngine.Events;
using GB.EventSystems;

namespace GB.Character.RPG
{	
	public class GB_HP : GB_RPGAttribute, GB_ILiveHandler
	{
		[Serializable]
		public struct Resistence
		{
			public string type;
			[Range(0, 1)] public float effect;
			public GameObject prefab;
		}

		[SerializeField] protected string demageId = "Demage";
        [SerializeField] protected string impcatId = "Impact";
		[SerializeField] protected string HpId = "HP";
		[SerializeField][Range(0, 1)] protected float blockEffekt;

		[SerializeField] protected Resistence[] resistence;
		
		[Header("Events")]
		[SerializeField] protected GB_TransformEvent emitOffender;
		[SerializeField] protected GB_FloatEvent emitDemage;
        [SerializeField] protected GB_FloatEvent emitImpact;
		[SerializeField] protected GB_FloatEvent emitHp;
		[SerializeField] protected GB_VecEvent emitHpVec;
		[SerializeField] protected UnityEvent emitKO;

		public GameObject Prefab { get; private set; }
		public bool Block { get; set; }

		private Vector3 hpScale = Vector3.one;

		protected override void ExtendedStart() {}

		public void Heal(float heal)
		{
			curr += heal;
			hpScale.x = Percentage();
			emitHp.Invoke(HpId, hpScale.x);
			emitHpVec.Invoke(hpScale);
		}

		public void Recover()
		{
			curr = max;
		}

		public void TakeDemage(string type, float demage)
		{
			foreach (var res in resistence)
			{
				if (res.type == type)
				{
					demage *= 1 - res.effect;
					Prefab = res.prefab;
				}
			}

			if (Block) demage *= 1 - blockEffekt;
			curr -= demage;
			hpScale.x = Percentage();
			emitDemage.Invoke(demageId, demage);
			emitHp.Invoke(HpId, hpScale.x);
			emitHpVec.Invoke(hpScale);
			if(curr == 0)
			{
				emitKO.Invoke();
			}
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
