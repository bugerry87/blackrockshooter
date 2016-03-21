using UnityEngine;

namespace GBAssets.Character
{
	public abstract class GB_ACharPhysic : MonoBehaviour
	{
		//Mode
		public bool update { get; set; }

		//Controls
		public Vector3 move { get; set; }
		public bool jump { get; set; }
		public bool action1 { get; set; }
		public bool action2 { get; set; }
        public bool action3 { get; set; }
		public bool walk { get; set; }
		public bool crouch { get; set; }
		public float focus { get; set; }
		public float fire { get; set; }

		//Movement
		public Vector3 movement { get; protected set; }
		public Vector3 forward { get; protected set; }
		public float turnAmount { get; protected set; }
		public float forwardAmount { get; protected set; }
		public float speed { get; protected set; }

		//Environment
		public GameObject contactObject { get; protected set; }
		public Vector3 contactPoint { get; protected set; }
		public Vector3 contactNormal { get; protected set; }
		public float up { get; protected set; }
		public float right { get; protected set; }
		public float fall { get; protected set; }
		public bool grounded { get; protected set; }
		public bool sliding { get; protected set; }
        public bool contact { get; protected set; }
        public bool skinContact { get; protected set; }

		//Influence
		public Vector3 velocity { get; set; }
	}
}

