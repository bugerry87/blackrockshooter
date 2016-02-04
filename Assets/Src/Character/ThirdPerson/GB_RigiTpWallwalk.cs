using System;
using UnityEngine;

namespace GBAssets.Character.ThirdPerson
{
	public class GB_RigiTpWallwalk : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
			[SerializeField] public string
                forward = "Forward",
                turn = "Turn",
                up = "Up",
                right = "Right",
                jump = "Jump",
                jumpLeg = "JumpLeg",
                fall = "Fall",
                slide = "Slide",
                ground = "Ground",
                contact = "Contact",
                grab = "Grab",
                climb = "Climb";
		}
		
		[SerializeField] Parameters parameters = new Parameters();

		[Range(0f, 10f)][SerializeField] float sensity = 0.1f;

		private float time;

		 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
                time = 0;
			}
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.fixGrounding = true;
				physic.applyWallwalk = true;

				animator.SetFloat(parameters.forward, physic.speed, sensity, Time.deltaTime);
				animator.SetFloat(parameters.turn, physic.turnAmount, sensity, Time.deltaTime);
				animator.SetFloat(parameters.up, physic.up, sensity, Time.deltaTime);
				animator.SetFloat(parameters.right, physic.right, sensity, Time.deltaTime);

                if (!physic.contact)
                    animator.SetFloat(parameters.fall, time -= Time.deltaTime);

				animator.SetBool(parameters.ground, physic.grounded);
				animator.SetBool(parameters.slide, physic.sliding);
				animator.SetBool(parameters.contact, physic.contact);
				animator.SetBool(parameters.jump, physic.jump);
                animator.SetBool(parameters.grab, physic.CheckEdge());
                animator.SetFloat(parameters.climb, physic.grab.distance);
			}
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.jumpLeg = physic.right < 0.5f ? -1 : 1;
				animator.SetFloat(parameters.jumpLeg, physic.jumpLeg, sensity, Time.deltaTime);
                animator.SetBool(parameters.grab, physic.CheckEdge());
			}
		}
	}
}
