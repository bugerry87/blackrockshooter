using System;
using UnityEngine;

namespace GB.Character.ThirdPerson
{
	public class GB_RigiTpHold : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
			[SerializeField]
			public string 
				forward = "Forward",
				turn = "Turn",
				crouch = "Crouch",
				slide = "Slide",
				ground = "Ground",
				airborne = "Airborne",
				contact = "Contact",
				fall = "Fall",
				jump = "Jump";
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
				physic.fixGrounding = true;
				physic.applyGravity = true;

				animator.SetFloat(parameters.forward, physic.speed * physic.forwardAmount, sensity, Time.deltaTime);
				animator.SetFloat(parameters.turn, physic.turnAmount, sensity, Time.deltaTime);
				animator.SetFloat(parameters.fall, 0);

				bool hasTo = physic.crouch || physic.CheckCrouch();
                animator.SetBool(parameters.crouch, hasTo);
				animator.SetBool(parameters.ground, physic.grounded);
				animator.SetBool(parameters.slide, !hasTo && physic.sliding);
				animator.SetBool(parameters.contact, physic.contact);
				animator.SetBool(parameters.jump, physic.jump);

				if (hasTo)
				{
					if (!physic.crouching)
					{
						physic.Crouch();
					}
				}
				else
				{
					if (physic.crouching)
					{
						physic.Standup();
					}
				}
			}
		}
	}
}
