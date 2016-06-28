using UnityEngine;

namespace GBAssets.EventSystems
{
	public class GB_StateMessenger : StateMachineBehaviour 
	{
        [SerializeField] protected string[] enterMessages = null;
        [SerializeField] protected string[] exitMessages = null;

		public GB_MessageEmitter emitter { get; private set; }

		bool init = false;

		bool Init(Animator animator)
        {
            if(init || (init = emitter || animator && (emitter = animator.GetComponent<GB_MessageEmitter>())))
			{
				return true;
			}
			else
			{
#if UNITY_EDITOR
				Debug.LogWarning("GB_MessageEmitter missing!");
#endif
				return false;
			}
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
			if (Init(animator))
			{
				foreach(var msg in enterMessages)
				{
					if(msg != null && msg.Length > 0) emitter.EmitMessage(animator.gameObject, msg);
				}
			}
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
			if (Init(animator))
			{
				foreach(var msg in exitMessages)
				{
					if(msg != null && msg.Length > 0) emitter.EmitMessage(animator.gameObject, msg);
				}
			}
        }
    }
}
