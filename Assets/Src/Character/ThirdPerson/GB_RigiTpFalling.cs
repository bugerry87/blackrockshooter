using System;
using UnityEngine;

namespace GB.Character.ThirdPerson
{
	public class GB_RigiTpFalling : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
			[SerializeField]
			public string 
				forward = "Forward",
				turn = "Turn",
				fall = "Fall",
				jumpLeg = "JumpLeg",
				slide = "Slide",
				ground = "Ground",
				airborne = "Airborne",
				contact = "Contact";
		}
		
		[SerializeField] Parameters parameters = new Parameters();

		[Range(0f, 10f)][SerializeField] float sensity = 0.1f;
		
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.StartFall();
				animator.SetBool(parameters.airborne, true);
			}
		}
		
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.applyTurn = true;
				physic.applyAirControl = true;
				physic.applyGravity = true;

				animator.SetFloat(parameters.forward, physic.speed, sensity, Time.deltaTime);
				animator.SetFloat(parameters.turn, physic.turnAmount, sensity, Time.deltaTime);
				
				animator.SetFloat(parameters.fall, physic.fall, sensity, Time.deltaTime);
				animator.SetFloat(parameters.jumpLeg, physic.jumpLeg * physic.jumpDot, sensity, Time.deltaTime);

				animator.SetBool(parameters.ground, physic.grounded);
				animator.SetBool(parameters.slide, physic.sliding);
				animator.SetBool(parameters.contact, physic.contact);
			}
		}
		
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.fixGrounding = true;
			}
		}
	}
}
