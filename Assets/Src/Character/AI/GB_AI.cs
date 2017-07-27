using System;
using UnityEngine;
using GB.Utils;

namespace GB.Character.AI
{
	[RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
	[RequireComponent(typeof (Animator))]
	public class GB_AI : GB_AdoptedTarget
	{
		protected static event Action<GameObject> Attack;

		public static void InvokeAttack(GameObject go)
		{
			if (Attack != null)
			{
				Attack(go);
			}
		}

		[Serializable]
		protected struct Parameters
		{
			public string
				forward,
				turn,
				right,
				crouch,
				jump,
				attack,
				demage;

			public Parameters(
				string forward = "Forward", 
				string turn = "Turn", 
				string right = "Right", 
				string crouch = "Crouch", 
				string jump = "Jump", 
				string attack = "Attack", 
				string demage = "Demage"
			) {
				this.forward = forward;
				this.turn = turn;
				this.right = right;
				this.crouch = crouch;
				this.jump = jump;
				this.attack = attack;
				this.demage = demage;
			}
		}

		[SerializeField] protected Parameters parameters = new Parameters();
		[Range(0f, 10f)][SerializeField] protected float sensity = 0.5f;
		[Range(0f, 1f)][SerializeField] protected float precision = 0.5f;
		[Range(0f, 1f)][SerializeField] protected float malignity = 0.5f;
		[Range(0, 10)][SerializeField] protected int rotSpeed = 5;
		[SerializeField] protected float minDistance = 4;
		[SerializeField] protected int requestCooldown = 500;
		[SerializeField] protected int attackCooldown = 500;
		[SerializeField] protected bool syncAttacks = true;

		public UnityEngine.AI.NavMeshAgent agent { get; private set; }
		public Animator animator { get; private set; }
		public Rigidbody rig { get; private set; }
		public GB_ActionScheduler scheduler { get; protected set; }
		public bool ko { get; set; }
		bool attack { get; set; }

		protected float sqrDist = 0;

		protected virtual void Start()
		{
			Attack += RequestAttack;
			scheduler = ScriptableObject.CreateInstance<GB_ActionScheduler>();
			agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
			StopAttack();
		}

		protected virtual void Follow()
		{
			agent.SetDestination(target.position);
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
				StopAttack();
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
			Vector3 dir = Vector3.ProjectOnPlane(target.position - animator.transform.position, Vector3.up);
			dir = dir == Vector3.zero ? transform.forward : dir.normalized;

			float forward = Vector3.Dot(animator.transform.forward, dir);
			float turn = Vector3.Dot(animator.transform.right, dir);
			float back = Vector3.Dot(target.forward, animator.transform.forward);
			float right = Vector3.Dot(target.right, animator.transform.forward);

			if(forward < 0)
			{
				turn = turn < 0 ? -1 : 1;
			}

			if (back < -0.5f)
			{
				right = right < 0f ? -5 : 1;
			}

			if (sqrDist < minDistance * minDistance)
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

			if (
				!agent.isOnOffMeshLink && 
				(attack || malignity == 0.0f || back > malignity) && 
				(precision == 0.0f || forward > precision)
			) {
				if (attack)
				{
					animator.SetTrigger(parameters.attack);
				}
				else
				{
					RequestAttack(target);
				}
			}
			else
			{
				animator.ResetTrigger(parameters.attack);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), rotSpeed * Time.deltaTime);
			}
		}

		protected virtual void DoUpdate()
		{
			if (target == null)
			{
				Idle();
			}
			else if ((sqrDist = (animator.transform.position - target.position).sqrMagnitude) > agent.stoppingDistance * agent.stoppingDistance)
			{
				Follow();
			}
			else
			{
				Manace();
			}
			animator.SetFloat(parameters.demage, 0, sensity, Time.deltaTime);
		}

		public void RequestAttack(GameObject source)
		{
			RequestAttack(source.transform);
		}

		public void RequestAttack(Transform source)
		{
			if (source == target)
			{
				RequestAttack();
			}
		}

		public void RequestAttack(IAsyncResult result = null)
		{
			if (syncAttacks)
			{
				scheduler.Request(target, DoAttack, attackCooldown, StopAttack);
			}
			else
			{
				scheduler.Request(this, DoAttack, attackCooldown, StopAttack);
			}
		}

		public void DoAttack()
		{
			attack = true;
		}

		public void StopAttack(IAsyncResult result = null)
		{
			attack = false;
		}

		public bool IsAlly(Transform other)
		{
			return other && other.tag == tag;
		}

		protected virtual void OnAnimatorMove()
		{
			if(!ko) DoUpdate();
			if (Time.deltaTime != 0) agent.velocity = animator.deltaPosition / Time.deltaTime;
		}

		protected override void OnTriggerStay(Collider other)
		{
			if (target == null && IsAlly(other.transform))
			{
				GB_AdoptedTarget adopted = other.GetComponent<GB_AdoptedTarget>();
				if(adopted && adopted.target)
				{
					target = adopted.target;
				}
			}
			else
			{
				base.OnTriggerStay(other);
			}
		}

		protected virtual void OnDestroy()
		{
			Attack -= RequestAttack;
		}
	}
}