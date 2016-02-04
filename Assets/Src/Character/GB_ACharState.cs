using UnityEngine;

namespace GBAssets.Character
{
	public abstract class GB_ACharState<T> : StateMachineBehaviour 
		where T : GB_ACharPhysic
	{
		public T physic {get; private set;}

		public bool HasPhysics(Animator animator)
		{
			if(physic == null && animator != null)
			{
				physic = animator.GetComponent<T>();
			}
			return physic != null;
		}
	}
}

