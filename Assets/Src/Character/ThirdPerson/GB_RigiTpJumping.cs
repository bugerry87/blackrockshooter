using System;
using UnityEngine;

namespace GB.Character.ThirdPerson
{
	public sealed class GB_RigiTpJumping : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
            [SerializeField]
            public string
                forward = "Forward",
                turn = "Turn",
                up = "Up",
                fall = "Fall",
				jump = "Jump",
                jumpLeg = "JumpLeg",
                slide = "Slide",
                ground = "Ground",
				airborne = "Airborne",
                contact = "Contact",
                grab = "Grab",
                climb = "Climb";
		}
		
		[SerializeField] Parameters parameters = new Parameters();
		[Range(0f, 10f)][SerializeField] float sensity = 0.1f;
		[SerializeField] float explosion = 500;
		[SerializeField] float range = 10;
		
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

				animator.SetFloat(parameters.forward, physic.speed * physic.jumpDot, sensity, Time.deltaTime);
				animator.SetFloat(parameters.turn, physic.turnAmount, sensity, Time.deltaTime);
				
				animator.SetFloat(parameters.fall, physic.fall, sensity, Time.deltaTime);
				animator.SetFloat(parameters.jumpLeg, physic.jumpLeg * physic.jumpDot, sensity, Time.deltaTime);
                animator.SetFloat(parameters.up, physic.up);

				animator.SetBool(parameters.jump, false);
				animator.SetBool(parameters.ground, physic.grounded);
				animator.SetBool(parameters.slide, physic.sliding);
				animator.SetBool(parameters.contact, physic.skinContact);

                animator.SetBool(parameters.grab, physic.CheckEdge());
                animator.SetFloat(parameters.climb, physic.grab.distance);
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
