using UnityEngine;

namespace GBAssets.Utils
{
	[RequireComponent(typeof(Animator))]
	public class GB_IKControl : MonoBehaviour {

		[SerializeField] Transform m_LookAt = null;
		[SerializeField] string[] m_LookAtStates = null;
		[SerializeField] float m_LookAtRay = 20;

		[SerializeField] Transform m_RightHand = null;
		[SerializeField] Transform m_LeftHand = null;
		[SerializeField] Transform m_RightFoot = null;
		[SerializeField] Transform m_LeftFoot = null;
		[SerializeField] float m_FootRay = 0.05f;


		private Animator animator = null;
		private GB_AdoptedTarget target = null;

		// Use this for initialization
		void Start ()
		{
			animator = GetComponent<Animator>();
			target = GetComponent<GB_AdoptedTarget>();

			if(m_LookAt == null && Camera.main != null){
				m_LookAt = Camera.main.transform;
			}
		}
		
		void OnAnimatorIK()
		{
			bool lookat = false;

			foreach(string state in m_LookAtStates)
			{
				if(animator.GetCurrentAnimatorStateInfo(0).IsName(state))
				{
					lookat = true;
					break;
				}
			}


			if(lookat)
			{
				if(target != null && target.adopted != null){
					animator.SetLookAtWeight(1);
					animator.SetLookAtPosition(target.adopted.position);
				}
				else if(m_LookAt != null)
				{
					float dot = Vector3.Dot(animator.transform.forward, m_LookAt.forward);
					
					animator.SetLookAtWeight(Mathf.Abs(dot));
					animator.SetLookAtPosition(m_LookAt.position + m_LookAt.forward * m_LookAtRay * Mathf.Max(0, dot));
				}
				else
				{
					animator.SetLookAtWeight(0);
				}
			}

			if(m_RightHand != null)
			{
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);  
				animator.SetIKPosition(AvatarIKGoal.RightHand, m_RightHand.position);
				animator.SetIKRotation(AvatarIKGoal.RightHand, m_RightHand.rotation);
			}
			else
			{          
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
			}

			if(m_LeftHand != null)
			{
				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);  
				animator.SetIKPosition(AvatarIKGoal.LeftHand, m_LeftHand.position);
				animator.SetIKRotation(AvatarIKGoal.LeftHand, m_LeftHand.rotation);
			}
			else
			{          
				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
			}

			if(m_RightFoot != null)
			{
				RaycastHit hit;
				if(Physics.Raycast(m_RightFoot.position, Vector3.down, out hit, m_FootRay))
				{
					animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
					//animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);  
					animator.SetIKPosition(AvatarIKGoal.RightFoot, hit.point);
					//animator.SetIKRotation(AvatarIKGoal.LeftFoot, m_LeftFoot.rotation);
				}
			}
			else
			{          
				animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
				animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
			}

			if(m_LeftFoot != null)
			{
				RaycastHit hit;
				if(Physics.Raycast(m_LeftFoot.position, Vector3.down, out hit, m_FootRay))
				{
					animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
					//animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);  
					animator.SetIKPosition(AvatarIKGoal.LeftFoot, hit.point);
					//animator.SetIKRotation(AvatarIKGoal.LeftFoot, m_LeftFoot.rotation);
				}
			}
			else
			{          
				animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
			}
		}
	}
}
