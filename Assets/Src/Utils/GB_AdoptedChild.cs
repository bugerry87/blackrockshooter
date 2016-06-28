using UnityEngine;

namespace GBAssets.Utils
{
    public class GB_AdoptedChild : GB_AUpdateMode
    {
        [SerializeField] protected Transform m_Target = null;
        [SerializeField] protected Vector3 localPosition = Vector3.zero;
        [SerializeField] protected Vector3 localRotation = Vector3.zero;
        [SerializeField] protected Vector3 localScale = Vector3.one;
		[SerializeField] protected float smoothSpeed = 0f;
		[SerializeField] protected float snapLimit = 0.1f;

		public Transform target { get { return m_Target; } protected set { m_Target = value; } }

        void Start ()
        {
            SetParent(target);
        }

        protected override void DoUpdate(float delta)
        {
			if (smoothSpeed == 0f)
			{
				transform.localPosition = localPosition;
				transform.localEulerAngles = localRotation;
				transform.localScale = localScale;
			}
			else
			{
				LerpLocal(delta);
			}
        }

		public void SetParent(Transform target)
		{
			this.target = target;

			transform.SetParent(target);
			if (updateType == UpdateType.ManualUpdate)
			{
				transform.localPosition = localPosition;
				transform.localEulerAngles = localRotation;
				transform.localScale = localScale;
			}
		}

		protected void LerpLocal(float delta)
		{
			float lerpDelta = (transform.localPosition - localPosition).sqrMagnitude + 
				(transform.localEulerAngles - localRotation).sqrMagnitude +
				(transform.localScale - localScale).sqrMagnitude;

			if (lerpDelta > snapLimit)
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, localPosition, smoothSpeed * delta);
				transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, localRotation, smoothSpeed * delta);
				transform.localScale = Vector3.Lerp(transform.localScale, localScale, smoothSpeed * delta);
			}
			else if(lerpDelta != 0)
			{
				transform.localPosition = localPosition;
				transform.localEulerAngles = localRotation;
				transform.localScale = localScale;
			}
		}
    }
}
