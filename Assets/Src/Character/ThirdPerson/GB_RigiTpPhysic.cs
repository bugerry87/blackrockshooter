using System;
using UnityEngine;

namespace GB.Character.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class GB_RigiTpPhysic : GB_ACharPhysic
	{
        public static readonly Vector3 XZ = new Vector3(1, 0, 1);
		public const int MAX_SLOPLIMIT = 90;

        [Serializable]
        struct HelperNames
        {
            public string
                wall,
                grab,
                push;

			public HelperNames(string wall = "WallHelper", string grab = "GrabHelper", string push = "PushAble")
			{
				this.wall = wall;
				this.grab = grab;
				this.push = push;
			}
        }

        [SerializeField] HelperNames m_Helpers = new HelperNames();
		[SerializeField][Range(0f, 1f)] protected float m_Skin = 0.5f;
		[SerializeField][Range(0f, 1f)] protected float m_Offset = 0.5f;
        [SerializeField][Range(1f, 2f)] protected float m_GrabWidth = 1.1f;
		[SerializeField][Range(0f, 2f)] protected float m_GrabHeight = 1.8f;
        [SerializeField][Range(0f, 2f)] protected float m_GrabRange = 0.9f;
		[SerializeField][Range(0, MAX_SLOPLIMIT)] protected int m_SlopeLimit = 44;
        [SerializeField][Range(0, MAX_SLOPLIMIT)] protected int m_WallUpLimit = 30;
        [SerializeField][Range(0, MAX_SLOPLIMIT)] protected int m_GrabEdge = 5;
		[SerializeField][Range(0f, 10f)] protected float m_GravityMultiplier = 1f;
		[SerializeField][Range(0f, 10f)] protected float m_SlideMultiplier = 1f;
		[SerializeField][Range(0f, 1f)] protected float m_CrouchRange = 0.5f;
		[SerializeField] protected float m_TurnSpeed = 360f;
		[SerializeField] protected float m_JumpHeight = 12f;
		[SerializeField] protected float m_JumpWidth = 12f;
		[SerializeField] protected float m_AirControl = 0.5f;
        [SerializeField] protected float m_MaxVelocity = 18f;

		public Vector3 jumpVector {get; protected set;}
		public Vector3 jumpDir {get; protected set;}
		public float jumpDot { get; protected set; }
		public float jumpLeg { get; set; }

		protected CapsuleCollider m_Capsule {get; set;}
		protected Rigidbody m_Rigidbody {get; set;}
		protected Vector3 m_CapsuleCenter {get; set;}
		protected float m_CapsuleHeight {get; set;}

		public bool crouching { get; protected set; }
        public bool push { get; protected set; }

		public bool applyGravity {get; set;}
		public bool applyTurn {get; set;}
		public bool applyMove { get; set; }
		public bool applyAirControl {get; set;}
		public bool applySliding {get; set;}
		public bool fixGrounding {get; set;}
		public bool applyJump {get; set;}
		public bool applyWallwalk {get; set;}

        protected float slope_limit = 0;
        protected float wallup_limit = 0;
        protected float grab_edge = 0;

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
				movement = Vector3.Scale(Vector3.ProjectOnPlane(move.sqrMagnitude > 1f ? move.normalized : move, contactNormal), XZ);
			else
                movement = Vector3.Scale(Vector3.Project(move.sqrMagnitude > 1f ? move.normalized : move, -contactNormal), XZ);

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
			contactObject = other.gameObject;
            ContactPoint[] contacts = other.contacts;
            Vector3 pos = transform.position + movement * m_Offset;

            foreach (ContactPoint c in contacts)
            {
                if(speed > 0 && contactObject.tag.StartsWith(m_Helpers.push))
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
				else if (def > 0.5f) //Backflip hack
				{
					jumpDot = 1;
                    jumpDir = -transform.forward;
                    jumpVector = jumpDir * m_JumpWidth;
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
					sliding = true;
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
					//ignore
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
			crouching = true;
		}
		
		public virtual bool CheckCrouch()
		{
			Vector3 pos = transform.position + Vector3.up * (m_CapsuleHeight - m_Capsule.radius) * transform.localScale.y;
			float rad = m_Capsule.radius * transform.localScale.z;

			return Physics.CheckSphere(pos, rad, 1);
		}

		public virtual void Standup()
		{
			if (!CheckCrouch())
			{
				m_Capsule.height = m_CapsuleHeight;
				m_Capsule.center = m_CapsuleCenter;
				crouching = false;
			}
		}

        public virtual bool CheckEdge()
        {
            Vector3 pos =
                transform.position +
                transform.forward * m_Capsule.radius * transform.localScale.z * m_GrabWidth +
                Vector3.up * m_GrabHeight;
            Ray ray = new Ray(pos, Vector3.down);

#if UNITY_EDITOR
            Debug.DrawRay(pos, Vector3.down, Color.red);
#endif
            return Physics.Raycast(ray, out grab, m_GrabRange, 1) && grab.normal.y > grab_edge;
        }
    }
}
