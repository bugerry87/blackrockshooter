using UnityEngine;
using GBAssets.Utils;

namespace GBAssets.CameraControl
{
	public sealed class GB_AntiCamCollider : GB_AUpdateMode
	{
        //It's recomanded to use FixedUpdate
		[SerializeField] Transform target = null; 
		[SerializeField] float min = 1.0f;
		[SerializeField] float max = 10.0f;
        [SerializeField] float destine = 5f;
		[SerializeField] float radius = 0.5f;
		[SerializeField] string zAxis = "Z";
		[SerializeField] int ignoreLayer = 9;
        [SerializeField] bool allowHiding = false; //needs Trigger!

		public float Destine { get { return destine; } set { destine = value; } }

        bool contact;

		void Awake()
		{
			if(target == null) enabled = false;
            radius = Mathf.Abs(radius);
		}
		
		protected override void DoUpdate(float deltaTime)
        {
            //Valid destine
            Destine = Mathf.Max(Destine, min);
			Destine = Mathf.Min(Destine, max);

            Ray ray = new Ray(target.position, -transform.forward);
            RaycastHit hit;

            //Looking for best position
            if(allowHiding && !contact)
            {
                transform.localPosition = Vector3.back * Destine;
            }
            else if(Physics.SphereCast(ray, radius, out hit, Destine, ignoreLayer))
            {
                transform.localPosition = Vector3.back * (hit.distance - radius);
			}
            else
            {
                transform.localPosition = Vector3.back * Destine;
            }
			
			//Change desire for next Update an reset contact
			Destine -= Input.GetAxisRaw(zAxis);
            contact = false;
        }
        
        void OnTriggerStay(Collider other)
		{
			if(other.isTrigger)
			{
				//ignore
			}
			else
			{
				contact = true;
			}
		}
	}
}