using UnityEngine;
using UnityEngine.Events;
using GBAssets.Utils;

namespace GBAssets.CameraControl
{
	public class GB_CamClamp : GB_AUpdateMode
	{
		public static GB_CamClamp curr { get; protected set; }
		
		[SerializeField][Range(0, byte.MaxValue)] protected byte priority = 0;
		[SerializeField] protected float smoothSpeed = 1f;
		[SerializeField] protected float snapLimit = 0.1f;
		[SerializeField] protected UnityEvent emitClamp;
		[SerializeField] protected UnityEvent emitUnclamp;

		protected Transform cam;

		void Start()
		{
			cam = Camera.main.transform;
			Claim();
		}

		protected override void DoUpdate(float deltaTime)
		{
			if (curr == this)
			{
				if (cam.parent != transform)
				{
					float lerpDelta = (cam.transform.position - transform.position).sqrMagnitude + Quaternion.Angle(cam.rotation, transform.rotation);
					if (lerpDelta > snapLimit)
					{
						cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position, smoothSpeed * deltaTime);
						cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, transform.rotation, smoothSpeed * deltaTime);
					}
					else
					{
						Clamp();
					}
				}
			}
			else if (curr == null)
			{
				Claim();
			}
		}

		public void Claim()
		{
			if (curr == null || curr.priority <= priority)
			{
				curr = this;
				if (updateType == UpdateType.ManualUpdate)
				{
					Clamp();
				}
				else
				{
					cam.SetParent(null);
				}
			}
		}

		public void Disclaim()
		{
			emitUnclamp.Invoke();
			curr = null;
		}

		public void Clamp()
		{
			cam.SetParent(transform);
			cam.localPosition = Vector3.zero;
			cam.localEulerAngles = Vector3.zero;
			cam.localScale = Vector3.one;
			emitClamp.Invoke();
		}

		void OnEnable()
		{
			if(cam) Claim();
		}

		void OnDisable()
		{
			Disclaim();
		}
	}
}
