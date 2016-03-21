using System;
using UnityEngine;
using GBAssets.Utils;

namespace GBAssets.Character.AI
{
	[RequireComponent(typeof (NavMeshAgent))]
	[RequireComponent(typeof (Animator))]
	[RequireComponent(typeof (GB_AdoptedTarget))]
	public class GB_AI : MonoBehaviour
	{
		[Serializable]
		protected class Parameters
		{
			[SerializeField]
			public string
				forward = "Forward",
				turn = "Turn",
				right = "Right",
				crouch = "Crouch",
				jump = "Jump",
				attack = "Attack";
		}

		[SerializeField] protected Parameters parameters = new Parameters();
		[Range(0f, 10f)][SerializeField] protected float sensity = 0.5f;
		[Range(0f, 1f)][SerializeField] protected float precision = 0.5f;
		[Range(0f, 1f)][SerializeField] protected float malignity = 0.5f;
		[Range(0, 10)][SerializeField] protected int rotSpeed = 5;

		[SerializeField] protected float minDistance = 4;

		public GB_AdoptedTarget target { get; private set; }
		public NavMeshAgent agent { get; private set; }
		public Animator animator { get; private set; }
		public Rigidbody rig { get; private set; }

		public bool ApplyAttack{ get; set; }

		protected float sqrDist = 0;

		protected virtual void Start()
		{
			target = GetComponent<GB_AdoptedTarget>();
			agent = GetComponent<NavMeshAgent>();
			animator = GetComponent<Animator>();
			rig = GetComponent<Rigidbody>();
		}

		protected virtual void Idle()
		{
			animator.SetFloat(parameters.forward, 0, sensity, Time.deltaTime);
			animator.SetFloat(parameters.turn, 0, sensity, Time.deltaTime);
			animator.SetFloat(parameters.right, 0, sensity, Time.deltaTime);
			animator.ResetTrigger(parameters.attack);
			transform.rotation = animator.rootRotation;
			ApplyAttack = false;
		}

		protected virtual void Follow()
		{
			agent.SetDestination(target.adopted.position);
			var path = agent.path;

			if(path.corners.Length >= 2)
			{
				Vector3 dir = (path.corners[1] - animator.transform.position).normalized;
				float forward = Vector3.Dot(animator.transform.forward, dir);
				float turn = Vector3.Dot(animator.transform.right, dir);

				if(forward < 0)
				{
					turn = turn < 0 ? -1 : 1;
				}
				
				animator.SetFloat(parameters.forward, forward, sensity, Time.deltaTime);
				animator.SetFloat(parameters.turn, turn, sensity, Time.deltaTime);
				animator.SetFloat(parameters.right, 0, sensity, Time.deltaTime);
				animator.SetBool(parameters.jump, agent.isOnOffMeshLink);
				animator.ResetTrigger(parameters.attack);
				transform.rotation = animator.rootRotation;
				ApplyAttack = false;
			}
			else
			{
				Idle();
			}

#if UNITY_EDITOR
			for (int i = 0; i < path.corners.Length-1; i++)
				Debug.DrawLine(path.corners[i], path.corners[i+1], Color.red);
#endif
		}

		protected virtual void Manace()
		{
			Vector3 dir = (target.adopted.position - animator.transform.position).normalized;
			float forward = Vector3.Dot(animator.transform.forward, dir);
			float turn = Vector3.Dot(animator.transform.right, dir);
			float back = Vector3.Dot(target.adopted.forward, animator.transform.forward);
			float right = Vector3.Dot(target.adopted.right, animator.transform.forward);

			if(forward < 0)
			{
				turn = turn < 0 ? -1 : 1;
			}

			if(back < -0.5f)
			{
				right = right < 0f ? -5 : 1;
			}

			if(sqrDist < minDistance * minDistance)
			{
				animator.SetFloat(parameters.forward, -1, sensity, Time.deltaTime);
			}
			else
			{
				animator.SetFloat(parameters.forward, 0, sensity, Time.deltaTime);
			}
			
			animator.SetFloat(parameters.turn, turn, sensity, Time.deltaTime);
			animator.SetFloat(parameters.right, right, sensity, Time.deltaTime);
			animator.SetBool(parameters.jump, agent.isOnOffMeshLink);

			if(!agent.isOnOffMeshLink && (ApplyAttack || back > malignity) && forward > precision)
			{
				animator.SetTrigger(parameters.attack);
			}
			else
			{
				animator.ResetTrigger(parameters.attack);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), rotSpeed * Time.deltaTime);
			}
		}

		protected virtual void DoUpdate()
		{
			if (target.adopted == null)
			{
				Idle();
			}
			else if ((sqrDist = (animator.transform.position - target.adopted.position).sqrMagnitude) > agent.stoppingDistance * agent.stoppingDistance)
			{
				Follow();
			}
			else
			{
				Manace();
			}
		}

		protected virtual void OnAnimatorMove()
		{
			DoUpdate();
			agent.velocity = animator.deltaPosition / Time.deltaTime;
		}
	}
}