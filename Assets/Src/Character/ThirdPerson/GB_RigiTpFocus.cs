using System;
using UnityEngine;
using GB.EventSystems;

namespace GB.Character.ThirdPerson
{
	public sealed class GB_RigiTpFocus : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
			[SerializeField] public string slide = "Slide";
			[SerializeField] public string ground = "Ground";
			[SerializeField] public string contact = "Contact";
            [SerializeField] public string focus = "Focus";
			[SerializeField] public string turn = "Turn";
		}

		[SerializeField] Parameters parameters = new Parameters();
        [SerializeField] float raycast = 100;
        [SerializeField][Range(0f, 1f)] float body = 1;
        [SerializeField][Range(0f, 1f)] float head = 1;
        [SerializeField][Range(0f, 1f)] float eyes = 1;
        [Range(0f, 10f)][SerializeField] float sensity = 0.1f;

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				animator.SetBool(parameters.ground, physic.grounded);
				animator.SetBool(parameters.slide, physic.sliding);
                animator.SetBool(parameters.contact, physic.skinContact);
                animator.SetFloat(parameters.focus, physic.focus, sensity, Time.deltaTime);
				animator.SetFloat(parameters.turn, physic.turnAmount, sensity, Time.deltaTime);
			}
		}

        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
            Camera cam = Camera.main;
            var main = animator.GetFloat(parameters.focus);

            if(cam != null && main > 0.02f)
            {
                Ray ray = cam.ScreenPointToRay(cam.pixelRect.size * 0.5f);
                RaycastHit hit;
                Vector3 lookAt;

                if (Physics.Raycast(ray, out hit, raycast, 1))
			    {
				    lookAt = hit.point;
			    }
			    else
			    {
				    lookAt = animator.transform.position + ray.direction * raycast;
			    }

                animator.SetLookAtWeight(main, body, head, eyes);
			    animator.SetLookAtPosition(lookAt);
            }
		}
    }
}
