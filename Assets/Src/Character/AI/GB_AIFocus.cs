using System;
using UnityEngine;

namespace GB.Character.AI
{
	public sealed class GB_AIFocus : StateMachineBehaviour
	{
		[SerializeField] Vector3 pivot;
        [SerializeField][Range(0f, 1f)] float body = 1;
        [SerializeField][Range(0f, 1f)] float head = 1;
        [SerializeField][Range(0f, 1f)] float eyes = 1;
        [Range(0f, 1)][SerializeField] float main = 1f;

        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var ai = animator.GetComponent<GB_AI>();
            if(ai && ai.target && main > 0.02f)
            {
				var offset = ai.target.TransformPoint(pivot);
                animator.SetLookAtWeight(main, body, head, eyes);
			    animator.SetLookAtPosition(offset);
            }
		}
    }
}
