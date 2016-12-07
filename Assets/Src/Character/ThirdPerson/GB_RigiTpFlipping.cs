using System;
using UnityEngine;

namespace GB.Character.ThirdPerson
{
	public class GB_RigiTpFlipping : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
            [SerializeField]
            public string
                forward = "Forward",
                turn = "Turn",
                up = "Up",
				jump = "Jump",
                jumpLeg = "JumpLeg",
			    fall = "Fall",
                slide = "Slide",
                ground = "Ground",
                contact = "Contact";
		}
		
		[SerializeField] Parameters parameters = new Parameters();
		[Range(0f, 10f)][SerializeField] float sensity = 0.1f;
		[SerializeField] float explosion = 500;
		[SerializeField] float range = 10;

		 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
                physic.applyJump = true;
				var rig = physic.contactObject.GetComponent<Rigidbody>();
				if(rig != null)
				{
					rig.AddExplosionForce(explosion, animator.transform.position, range);
				}
			}
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.applyAirControl = true;
				physic.applyGravity = true;
                physic.jumpLeg = physic.speed;

                animator.SetFloat(parameters.forward, 0, sensity, Time.deltaTime);
                animator.SetFloat(parameters.turn, 0, sensity, Time.deltaTime);
                animator.SetFloat(parameters.jumpLeg, physic.jumpLeg, sensity, Time.deltaTime);
				animator.SetFloat(parameters.fall, physic.fall, sensity, Time.deltaTime);

                animator.SetFloat(parameters.up, physic.up);

                animator.SetBool(parameters.jump, physic.jump);
				animator.SetBool(parameters.ground, physic.grounded);
				animator.SetBool(parameters.slide, physic.sliding);
				animator.SetBool(parameters.contact, physic.skinContact);
			}
		}
		
	}
}
