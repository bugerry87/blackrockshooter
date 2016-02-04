using System;
using UnityEngine;
using GBAssets.EventSystems;

namespace GBAssets.Character.ThirdPerson
{
	public class GB_RigiTpFocus : GB_ACharState<GB_RigiTpPhysic>
	{
		[Serializable]
		class Parameters
		{
			[SerializeField] public string slide = "Slide";
			[SerializeField] public string ground = "Ground";
			[SerializeField] public string contact = "Contact";
            [SerializeField] public string focus = "Focus";
		}

		[SerializeField] Parameters parameters = new Parameters();
        [SerializeField] float raycast = 100;
        [SerializeField] float body = 1;
        [SerializeField] float head = 1;
        [SerializeField] float eyes = 1;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SendMessage("OnMessage", "SpawnShooter");
        }

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(HasPhysics(animator))
			{
				animator.SetBool(parameters.ground, physic.grounded);
				animator.SetBool(parameters.slide, physic.sliding);
                animator.SetBool(parameters.contact, physic.skinContact);
                animator.SetBool(parameters.focus, physic.fire2);
			}
		}

        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
            Camera cam = Camera.main;
            if(cam != null)
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

                animator.SetLookAtWeight(1, body, head, eyes);
			    animator.SetLookAtPosition(lookAt);
            }
		}

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SendMessage("OnMessage", "DespawnShooter");
        }
	}
}
