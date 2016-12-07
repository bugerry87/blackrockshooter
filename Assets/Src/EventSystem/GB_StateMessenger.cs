using UnityEngine;

namespace GB.EventSystems
{
	public class GB_StateMessenger : StateMachineBehaviour 
	{
        [SerializeField] protected string[] enterMessages = null;
        [SerializeField] protected string[] exitMessages = null;

		public GB_MessageEmitter emitter { get; private set; }

		bool Init(Animator animator)
        {
			if (emitter == null)
			{
				emitter = animator.GetComponent<GB_MessageEmitter>();
#if UNITY_EDITOR
				if(emitter == null)
					Debug.LogWarning("GB_MessageEmitter missing!");
#endif
			}
			return emitter != null;
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
