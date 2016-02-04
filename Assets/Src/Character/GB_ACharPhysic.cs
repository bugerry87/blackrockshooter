using UnityEngine;

namespace GBAssets.Character
{
	public abstract class GB_ACharPhysic : MonoBehaviour
	{
		public static readonly Vector3 XZ = new Vector3(1, 0, 1);
		public const int MAX_SLOPLIMIT = 90;

		//Mode
		public bool update { get; set; }

		//Controls
		public Vector3 move { get; set; }
		public bool jump { get; set; }
		public bool crouch { get; set; }
		public bool walk { get; set; }
		public bool fire1 { get; set; }
		public bool fire2 { get; set; }
		public bool action1 { get; set; }
		public bool action2 { get; set; }

		//Movement
		public Vector3 movement { get; protected set; }
		public Vector3 forward { get; protected set; }
		public float turnAmount { get; protected set; }
		public float forwardAmount { get; protected set; }
		public float speed { get; protected set; }

		//Environment
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

