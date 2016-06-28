using System;
using UnityEngine;

namespace GBAssets.Character.ThirdPerson
{
	public sealed class GB_RigiTpClimbing : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		struct Parameters
		{
            public string
                forward,
                up,
                grab,
                climb;

			public Parameters(string forward = "Forward", string up = "Up", string grab = "Grab", string climb = "Climb")
			{
				this.forward = forward;
				this.up = up;
				this.grab = grab;
				this.climb = climb;
			}
		}

        [SerializeField]
        Parameters parameters = new Parameters();

        [Range(0f, 10f)]
        [SerializeField]
        float sensity = 0.1f;
        
        [SerializeField]
		AvatarTarget target = AvatarTarget.RightFoot;        

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

        // Occurs errors since since 5.3.x
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (HasPhysics(animator))
            {
                if (!animator.IsInTransition(layerIndex))
                {
                    animator.MatchTarget(physic.grab.point, Quaternion.identity, target, mask, start, goal);
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
