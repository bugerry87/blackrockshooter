using System;
using UnityEngine;

namespace GBAssets.Character.ThirdPerson
{
    public sealed class GB_RigiTpMaritalArt : GB_ACharState<GB_RigiTpPhysic> {

        [Serializable]
		class Parameters
		{
			[SerializeField]
			public string
				action1 = "Action1",
                action2 = "Action2",
                hold = "Hold";
		}

        [SerializeField] Parameters parameters = new Parameters();

        private bool hold = false;

	    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	        if(HasPhysics(animator))
			{
                if(hold)
                {
                    //ignore
                }
                else if(physic.action1)
                {
                    animator.SetTrigger(parameters.action1);
                }
                else if(physic.action2)
                {
                    animator.SetTrigger(parameters.action2);
                }
                hold = physic.action1 || physic.action2;
                animator.SetBool(parameters.hold, physic.action1 || physic.action2);             
			}
	    }
    }
}
