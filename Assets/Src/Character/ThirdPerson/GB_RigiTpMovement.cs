using System;
using UnityEngine;

namespace GB.Character.ThirdPerson
{
	public sealed class GB_RigiTpMovement : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
			[SerializeField]
			public string
				forward = "Forward",
				turn = "Turn",
				up = "Up",
				right = "Right",
				crouch = "Crouch",
				jump = "Jump",
				jumpLeg = "JumpLeg",
				slide = "Slide",
				ground = "Ground",
				airborne = "Airborne",
				contact = "Contact",
				push = "Push",
				grab = "Grab",
				climb = "Climb",
				fall = "Fall";
		}

		[SerializeField] Parameters parameters = new Parameters();

		[Range(0f, 10f)][SerializeField] float sensity = 0.1f;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				animator.SetBool(parameters.airborne, false);
			}
		}
		
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.applyTurn = true;
				physic.applyMove = true;
				physic.fixGrounding = true;
				physic.applyGravity = true;

                if (physic.contact)
                    physic.jumpLeg = (Mathf.Repeat(stateInfo.normalizedTime + 0.5f, 1) < 0.5f ? 1f : -1f) * physic.speed;

                animator.SetFloat(parameters.forward, physic.speed, sensity, Time.deltaTime);
                animator.SetFloat(parameters.turn, physic.turnAmount, sensity, Time.deltaTime);
                animator.SetFloat(parameters.up, physic.up, sensity, Time.deltaTime);
                animator.SetFloat(parameters.right, physic.right, sensity, Time.deltaTime);
                animator.SetFloat(parameters.jumpLeg, physic.jumpLeg);

                animator.SetBool(parameters.crouch, physic.crouch || physic.crouching);
                animator.SetBool(parameters.ground, physic.grounded);
                animator.SetBool(parameters.slide, physic.sliding);
                animator.SetBool(parameters.contact, physic.contact);
                animator.SetBool(parameters.jump, physic.jump);
                animator.SetBool(parameters.push, physic.push);
                animator.SetBool(parameters.grab, physic.CheckEdge());
                animator.SetFloat(parameters.climb, physic.grab.distance);
				animator.SetFloat(parameters.fall, 0);
			}
		}
	}
}
