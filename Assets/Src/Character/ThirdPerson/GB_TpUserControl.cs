using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace GBAssets.Character.ThirdPerson
{
    [RequireComponent(typeof (GB_ACharPhysic))]
    public class GB_TpUserControl : MonoBehaviour
    {
		[SerializeField] Transform m_RelativeTo = null;
		[SerializeField] string m_ForwardAxis = "Horizontal";
		[SerializeField] string m_SidewardAxis = "Vertical";
		[SerializeField] string m_JumpButton = "Jump";
		[SerializeField] string m_WalkButton = "Fire2";
		[SerializeField] string m_CrouchButton = "Fire3";

		public GB_ACharPhysic tp_physic { get; private set; } 	// A reference to the ThirdPersonCharacter on the object
		public Vector3 forward { get; private set; }             		// The current forward direction of the camera
		public Vector3 move { get; private set; }
		public float h { get; private set; }
		public float v { get; private set; }
		public bool jump { get; private set; }
		public bool crouch { get; private set; }
		public bool walk { get; private set; }
        
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
			h = CrossPlatformInputManager.GetAxis(m_ForwardAxis);
			v = CrossPlatformInputManager.GetAxis(m_SidewardAxis);
			jump = CrossPlatformInputManager.GetButtonDown(m_JumpButton); 
			crouch = CrossPlatformInputManager.GetButton(m_CrouchButton);
			walk = CrossPlatformInputManager.GetButton(m_WalkButton);

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
        }
    }
}
