using System;
using UnityEngine;
using GBAssets.Character;

namespace GBAssets.Character.ThirdPerson
{
	public class GB_RigiTpClimbing : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
            [SerializeField]
            public string
                forward = "Forward",
                up = "Up",
                grab = "Grab",
                climb = "Climb";
		}

        [SerializeField]
        Parameters parameters = new Parameters();

        [Range(0f, 10f)]
        [SerializeField]
        float sensity = 0.1f;

        [Range(0f, 1f)]
        [SerializeField]
        float start = 0;

        [Range(0f, 1f)]
        [SerializeField]
        float goal = 1f;

        MatchTargetWeightMask mask = new MatchTargetWeightMask(Vector3.up, 0);

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				animator.SetFloat(parameters.forward, physic.speed, sensity, Time.deltaTime);
                animator.SetFloat(parameters.up, physic.up);
			}
		}

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (HasPhysics(animator))
            {
                if (!animator.IsInTransition(layerIndex))
                {
                    animator.MatchTarget(physic.grab.point, Quaternion.identity, AvatarTarget.RightFoot, mask, start, goal);
                }
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (HasPhysics(animator))
            {
                animator.SetBool(parameters.grab, false);
            }
        }
	}
}
