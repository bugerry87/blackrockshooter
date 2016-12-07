using UnityEngine;

namespace GB.Character.ThirdPerson
{
    [RequireComponent(typeof (GB_ACharPhysic))]
    public class GB_TpUserControl : MonoBehaviour
    {
		[SerializeField] Transform m_RelativeTo = null;
		[SerializeField] string m_ForwardAxis = "Horizontal";
		[SerializeField] string m_SidewardAxis = "Vertical";
		[SerializeField] string m_JumpButton = "Jump";
		[SerializeField] string m_WalkButton = "Walk";
		[SerializeField] string m_CrouchButton = "Crouch";
        [SerializeField] string m_ActionButton1 = "Action1";
        [SerializeField] string m_ActionButton2 = "Action2";
        [SerializeField] string m_FocusButton = "Focus";
        [SerializeField] string m_DefButton = "Def";
		
		public GB_ACharPhysic tp_physic { get; private set; }
		public Vector3 forward { get; private set; }             		
		public Vector3 move { get; private set; }
		public float h { get; private set; }
		public float v { get; private set; }
		public bool jump { get; private set; }
		public bool crouch { get; private set; }
		public bool walk { get; private set; }
        public float def { get; private set; }
        public float focus { get; private set; }
        public bool actionX { get; private set; }
        public bool actionY { get; private set; }
        
        private void Start()
        {
			if (m_RelativeTo == null && Camera.main == null)
			{
				Debug.LogWarning ("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
			}
			else
			{
				m_RelativeTo = Camera.main.transform;
			}
            // get the third person character ( this should never be null due to require component )
            tp_physic = GetComponent<GB_ACharPhysic>();
        }

		private void Update()
		{
			// read inputs
			h = Input.GetAxis(m_ForwardAxis);
			v = Input.GetAxis(m_SidewardAxis);
			jump = Input.GetButtonDown(m_JumpButton) || Input.GetAxis(m_JumpButton) != 0; 
			crouch = Input.GetButton(m_CrouchButton) || Input.GetAxis(m_CrouchButton) != 0;
			walk = Input.GetButton(m_WalkButton) || Input.GetAxis(m_WalkButton) != 0;
            def = Input.GetButton(m_DefButton) ? 1 : Input.GetAxis(m_DefButton);
            focus = Input.GetButton(m_FocusButton) ? 1 : Input.GetAxis(m_FocusButton);
            actionX = Input.GetButton(m_ActionButton1) || Input.GetAxis(m_ActionButton1) != 0;
            actionY = Input.GetButton(m_ActionButton2) || Input.GetAxis(m_ActionButton2) != 0;

            // calculate move direction to pass to character
            if (m_RelativeTo != null)
            {
                // calculate camera relative direction to move:
                forward = Vector3.ProjectOnPlane(m_RelativeTo.forward, Vector3.up).normalized;
				move = v * forward + h * m_RelativeTo.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                move = v * Vector3.forward + h * Vector3.right;
            }

            // pass all parameters to the character control script
			tp_physic.move = move;
			tp_physic.crouch = crouch;
			tp_physic.walk = walk;
			tp_physic.jump |= jump;
            tp_physic.def = def;
            tp_physic.focus = focus;
            tp_physic.action1 = actionX;
            tp_physic.action2 = actionY;
        }
    }
}
