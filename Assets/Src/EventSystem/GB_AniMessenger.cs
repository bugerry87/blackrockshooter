using UnityEngine;

namespace GBAssets.EventSystems
{
	public class GB_AniMessenger : StateMachineBehaviour 
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
