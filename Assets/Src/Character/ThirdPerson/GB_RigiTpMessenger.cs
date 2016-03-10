using UnityEngine;
using GBAssets.EventSystems;

namespace GBAssets.Character.ThirdPerson
{
	public class GB_RigiTpMessenger : GB_ACharState<GB_RigiTpPhysic>
	{
        [SerializeField] protected string[] enterMessages = null;
        [SerializeField] protected string[] exitMessages = null;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach(var msg in enterMessages)
            {
                if(msg != null && msg.Length > 0) GB_MessageEvent.Emit(msg);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach(var msg in exitMessages)
            {
                if(msg != null && msg.Length > 0) GB_MessageEvent.Emit(msg);
            }
        }
    }
}
