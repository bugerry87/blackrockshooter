using System;
using UnityEngine;

namespace GBAssets.Character.ThirdPerson
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

		 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
                physic.applyJump = true;
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

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				//physic.fixGrounding = true;
			}
		}
	}
}
