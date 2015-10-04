using System;
using UnityEngine;

namespace GBAssets.Character.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class GB_RigiTpPhysic : GB_ACharPhysic
	{
        [Serializable]
        class HelperNames
        {
            [SerializeField]
            public string
                wall = "WallHelper",
                grab = "GrabHelper",
                push = "PushAble";
        }

        [SerializeField] HelperNames m_Helpers = new HelperNames();
		[Range(0f, 1f)][SerializeField] float m_Skin = 0.5f;
		[Range(0f, 1f)][SerializeField] float m_Offset = 0.5f;
        [Range(1f, 2f)][SerializeField] float m_GrabWidth = 1.1f;
		[Range(0, MAX_SLOPLIMIT)][SerializeField] int m_SlopeLimit = 44;
        [Range(0, MAX_SLOPLIMIT)][SerializeField] int m_WallUpLimit = 30;
        [Range(0, MAX_SLOPLIMIT)][SerializeField] int m_GrabEdge = 5;
		[Range(0f, 10f)][SerializeField] float m_GravityMultiplier = 1f;
		[Range(0f, 10f)][SerializeField] float m_SlideMultiplier = 1f;
		[Range(0f, 1f)][SerializeField] float m_CrouchRange = 0.5f;
        [Range(0f, 1.5f)][SerializeField] float m_GrabRange = 0.9f;
		[SerializeField] float m_TurnSpeed = 360f;
		[SerializeField] float m_JumpHeight = 12f;
		[SerializeField] float m_JumpWidth = 12f;
		[SerializeField] float m_AirControl = 0.5f;
        [SerializeField] float m_MaxVelocity = 18f;

		public Vector3 jumpVector {get; protected set;}
		public Vector3 jumpDir {get; protected set;}
		public float jumpDot { get; protected set; }
		public float jumpLeg { get; set; }

		protected CapsuleCollider m_Capsule {get; private set;}
		protected Rigidbody m_Rigidbody {get; private set;}
		protected Vector3 m_CapsuleCenter {get; private set;}
		protected float m_CapsuleHeight {get; private set;}

        public bool push { get; private set; }

		public bool applyGravity {get; set;}
		public bool applyTurn {get; set;}
		public bool applyMove { get; set; }
		public bool applyAirControl {get; set;}
		public bool applySliding {get; set;}
		public bool fixGrounding {get; set;}
		public bool applyJump {get; set;}
		public bool applyWallwalk {get; set;}

        private float slope_limit = 0;
        private float wallup_limit = 0;
        private float grab_edge = 0;

        public RaycastHit grab;

		void Start()
		{
			m_Capsule = GetComponent<CapsuleCollider>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			contactNormal = Vector3.up;
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;
		}

        void Awake()
        {
            //m_Rigidbody.maxDepenetrationVelocity = m_MaxVelocity;
            slope_limit = Mathf.Cos(m_SlopeLimit * Mathf.Deg2Rad);
            wallup_limit = Mathf.Cos(m_WallUpLimit * Mathf.Deg2Rad);
            grab_edge = Mathf.Cos(m_GrabEdge * Mathf.Deg2Rad);
        }

		void FixedUpdate()
		{
            up = -Vector3.Dot(contactNormal, transform.forward);
            right = Vector3.Dot(contactNormal, transform.right);

            if (!contact)
                movement = Vector3.Scale(move.sqrMagnitude > 1f ? move.normalized : move, XZ);
            else if (up < wallup_limit)
				movement = Vector3.Scale(Vector3.ProjectOnPlane(move.magnitude > 1f ? move.normalized : move, contactNormal), XZ);
			else
                movement = Vector3.Scale(Vector3.Project(move.magnitude > 1f ? move.normalized : move, -contactNormal), XZ);

#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawRay(m_Rigidbody.position, m_Rigidbody.velocity, Color.green);
            Debug.DrawRay(contactPoint, contactNormal);
            Debug.DrawRay(transform.position + Vector3.up, movement, Color.cyan);
#endif

			forward = movement != Vector3.zero ? movement.normalized : transform.forward;
			forwardAmount = Vector3.Dot(forward, transform.forward);
			turnAmount = Vector3.Dot(forward, transform.right);
			speed = movement.magnitude;

            skinContact = (contactPoint - transform.position).magnitude < m_Skin;
            grounded = skinContact && contactNormal.y > slope_limit;
            sliding = !grounded && skinContact;

			if(!contact || sliding)
			    fall = m_Rigidbody.velocity.y;

			if(walk && speed > 0.5f)
				speed = 0.5f;

			Apply();

            velocity += m_Rigidbody.velocity;
            m_Rigidbody.velocity = velocity.magnitude < m_MaxVelocity ? velocity : velocity.normalized * m_MaxVelocity;

			//cancel input
			velocity = Vector3.zero;
			contact = false;
            jump = false;
            push = false;
			update = true;
		}

		void OnCollisionStay(Collision other)
		{
            if (other.gameObject.layer != 8)
            {
                ContactPoint[] contacts = other.contacts;
                Vector3 pos = transform.position + movement * m_Offset;

                foreach (ContactPoint c in contacts)
                {
                    if(speed > 0 && other.gameObject.name.StartsWith(m_Helpers.push))
                    {
                        contactPoint = c.point;
                        contactNormal = c.normal;
                        break;
                    }
                    else if (c.normal.y >= 0 && (c.point - pos).sqrMagnitude < (contactPoint - pos).sqrMagnitude)
                    {
                        contactPoint = c.point;
                        contactNormal = c.normal;
                    }

                    grounded |= c.normal.y > slope_limit;
                }
            }
            contact = true;
		}

        void OnTriggerStay(Collider other)
        {
            if (!applyJump && other.name.StartsWith(m_Helpers.wall))
            {
                sliding = true;
            }

            if (other.name.StartsWith(m_Helpers.push))
            {
                push = true;
            }
        }

		//Applies
		protected virtual void Apply(){

			if(applyTurn)
			{
                if (forwardAmount * speed < 0f)
                {
                    turnAmount = turnAmount < 0 ? -1 : 1;
                }
				transform.Rotate(0, turnAmount * m_TurnSpeed * Time.fixedDeltaTime, 0);
			}

			if(applyGravity)
			{
				velocity += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
			}

			if(applySliding)
			{
                velocity += Physics.gravity * m_SlideMultiplier * Time.fixedDeltaTime;
			}

            if (applyWallwalk && m_Rigidbody.velocity.y < 0)
            {
                velocity += Vector3.down * m_Rigidbody.velocity.y;
            }

			if(applyJump)
			{
                if (sliding && !grounded)
                {
                    jumpDot = 1;
                    jumpDir = Vector3.Scale(contactNormal, XZ);
                    jumpVector = jumpDir * m_JumpWidth * (sliding ? 1 : speed);
                    velocity += jumpVector + Vector3.up * m_JumpHeight;
                }
                else
                {
                    jumpDot = 1;
                    jumpDir = forward;
                    jumpVector = jumpDir * m_JumpWidth * speed;
                    velocity += jumpVector + Vector3.up * m_JumpHeight;
                }
			}
			
			if(applyAirControl)
			{
                if (contact)
                {
                    //jumpLeg = 0;
                }
                else
                {
                    jumpDot = Mathf.Max(0, Vector3.Dot(jumpDir, forward));
                    velocity += jumpVector;
                    velocity += movement * m_AirControl;
                }
			}

			if(fixGrounding)
			{
				if(applyAirControl || applyJump || sliding)
				{
				}
				else if(grounded && !contact)
				{
					m_Rigidbody.velocity = Vector3.ProjectOnPlane(m_Rigidbody.velocity, contactNormal) + Physics.gravity;
				}
				else
				{
					m_Rigidbody.velocity = Vector3.ProjectOnPlane(m_Rigidbody.velocity, contactNormal);
				}
			}

			applyGravity = false;
			applyTurn = false;
			applyAirControl = false;
			applySliding = false;
			applyWallwalk = false;
			fixGrounding = false;
			applyJump = false;
		}
		
		public virtual void StartFall()
		{
			jumpDot = 1;
			jumpDir = movement;
			jumpVector = movement;
		}		

		public virtual void Crouch()
		{
			m_Capsule.height = m_CapsuleHeight * m_CrouchRange;
			m_Capsule.center = m_CapsuleCenter * m_CrouchRange;
		}
		
		public virtual bool CheckCrouch()
		{
			Vector3 pos = transform.position + Vector3.up * (m_CapsuleHeight - m_Capsule.radius) * transform.localScale.y;
			float rad = m_Capsule.radius * transform.localScale.z;

			return Physics.CheckSphere(pos, rad, 1);
		}

		public virtual void Standup()
		{
			m_Capsule.height = m_CapsuleHeight;
			m_Capsule.center = m_CapsuleCenter;
		}

        public virtual bool CheckEdge()
        {
            float height = m_CapsuleHeight * transform.localScale.y;
            Vector3 pos =
                transform.position +
                transform.forward * m_Capsule.radius * transform.localScale.z * m_GrabWidth +
                Vector3.up * height;
            Ray ray = new Ray(pos, Vector3.down);

#if UNITY_EDITOR
            Debug.DrawRay(pos, Vector3.down, Color.red);
#endif
            return Physics.Raycast(ray, out grab, height * m_GrabRange, 1) && grab.normal.y > grab_edge;
        }
	}
}
