using System;
using UnityEngine;

namespace GBAssets.Character.ThirdPerson
{
	public class GB_RigiTpCrouching : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
			[SerializeField] public string forward = "Forward";
			[SerializeField] public string turn = "Turn";
			[SerializeField] public string crouch = "Crouch";
			[SerializeField] public string slide = "Slide";
			[SerializeField] public string ground = "Ground";
			[SerializeField] public string contact = "Contact";
		}

		[SerializeField] Parameters parameters = new Parameters();
		[Range(0f, 10f)][SerializeField] float sensity = 0.1f;

		 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.Crouch();
			}
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.applyTurn = true;
				physic.fixGrounding = true;
				physic.applyGravity = true;

				animator.SetFloat(parameters.forward, physic.speed, sensity, Time.deltaTime);
				animator.SetFloat(parameters.turn, physic.turnAmount, sensity, Time.deltaTime);

                animator.SetBool(parameters.crouch, physic.crouch || physic.CheckCrouch());
				animator.SetBool(parameters.ground, physic.grounded);
				animator.SetBool(parameters.slide, physic.sliding);
				animator.SetBool(parameters.contact, physic.contact);
			}
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.Standup();
			}
		}
	}
}
