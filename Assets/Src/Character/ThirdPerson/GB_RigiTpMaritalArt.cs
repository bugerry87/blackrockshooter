using System;
using UnityEngine;
using GBAssets.Character.AI;

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
                hold = "Hold",
				def = "Def";
		}

        [SerializeField] Parameters parameters = new Parameters();
		[Range(0f, 10f)][SerializeField] float sensity = 0.1f;
		
        private bool hold = false;

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
					GB_AI.InvokeAttack(animator.gameObject);		
                }
                else if(physic.action2)
                {
                    animator.SetTrigger(parameters.action2);
					GB_AI.InvokeAttack(animator.gameObject);	
                }
				
				hold = physic.action1 || physic.action2;
                animator.SetBool(parameters.hold, physic.action1 != physic.action2);
				animator.SetFloat(parameters.def, physic.def, sensity, Time.deltaTime);            
			}
	    }
    }
}
