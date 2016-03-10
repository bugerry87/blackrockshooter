using System;
using UnityEngine;

namespace GBAssets.Character.ThirdPerson
{
	public sealed class GB_RigiTpSliding : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
			[SerializeField] public string forward = "Forward";
			[SerializeField] public string turn = "Turn";
			[SerializeField] public string up = "Up";
			[SerializeField] public string right = "Right";
			[SerializeField] public string fall = "Fall";
			[SerializeField] public string jump = "Jump";
			[SerializeField] public string jumpLeg = "JumpLeg";
			[SerializeField] public string slide = "Slide";
			[SerializeField] public string ground = "Ground";
			[SerializeField] public string contact = "Contact";
		}

		[SerializeField] Parameters parameters = new Parameters();

		[Range(0f, 10f)][SerializeField] float sensity = 0.1f;

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				physic.applyTurn = true;
				physic.applySliding = true;
				physic.applyGravity = true;

				physic.jumpLeg = physic.right < 0.5f ? -1 : 1;

				animator.SetFloat(parameters.forward, physic.speed, sensity, Time.deltaTime);
				animator.SetFloat(parameters.turn, physic.turnAmount, sensity, Time.deltaTime);
				animator.SetFloat(parameters.up, physic.up, sensity, Time.deltaTime);
				animator.SetFloat(parameters.right, physic.right, sensity, Time.deltaTime);
				animator.SetFloat(parameters.fall, physic.fall, sensity, Time.deltaTime);
				animator.SetFloat(parameters.jumpLeg, physic.jumpLeg, sensity, Time.deltaTime);

				animator.SetBool(parameters.ground, physic.grounded);
				animator.SetBool(parameters.slide, physic.sliding);
                animator.SetBool(parameters.contact, physic.skinContact);
				animator.SetBool(parameters.jump, physic.jump);
			}
		}
	}
}
