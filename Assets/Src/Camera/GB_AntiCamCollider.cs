using UnityEngine;
using GBAssets.Utils;

namespace GBAssets.CameraControl
{
	[RequireComponent(typeof(Camera))]
	public sealed class GB_AntiCamCollider : GB_AUpdateMode
	{
        //It's recomanded to use FixedUpdate
		[SerializeField] Transform target = null; 
		[SerializeField] float min = 1.0f;
		[SerializeField] float max = 10.0f;
        [SerializeField] float destine = 5f;
		[SerializeField] float radius = 0.5f;
		[SerializeField] string zAxis = "Z";
		[SerializeField] int ignoreLayer = 8;
        [SerializeField] bool allowHiding = false; //needs Trigger!

        bool contact;

		void Awake()
		{
			if(target == null) enabled = false;
			destine = max;
            radius = Mathf.Abs(radius);
		}
		
		protected override void DoUpdate(float deltaTime)
        {
            //Valid destine
            destine = Mathf.Max(destine, min);
			destine = Mathf.Min(destine, max);

            Ray ray = new Ray(target.position, -transform.forward);
            RaycastHit hit;

            //Looking for best position
            if(allowHiding && !contact)
            {
                transform.localPosition = Vector3.back * destine;
            }
            else if(Physics.SphereCast(ray, radius, out hit, destine, ignoreLayer))
            {
                transform.localPosition = Vector3.back * (hit.distance - radius);
			}
            else
            {
                transform.localPosition = Vector3.back * destine;
            }
			
			//Change desire for next Update an reset contact
			destine -= Input.GetAxisRaw(zAxis);
            contact = false;
        }
        
        void OnTriggerStay(Collider other)
		{
			if(other.isTrigger)
			{
				//ignore
			}
			else if(other.gameObject.layer == ignoreLayer) 
			{
				//ignore
			}
			else
			{
				contact = true;
			}
		}

        public void SetDestine(float destine)
        {
            this.destine = destine;
        }

        public float GetDestine()
        {
            return destine;
        }
	}
}