using System.Collections.Generic;
using UnityEngine;


//Obsolete!!!
namespace GBAssets.CameraControl
{
	[RequireComponent(typeof(Camera))]
	public class GB_CamCollidePreventer : MonoBehaviour
	{
		[SerializeField] Transform target = null; 
		[SerializeField] float min = 1.0f;
		[SerializeField] float max = 10.0f;
        [SerializeField] float destine = 5f;
		[SerializeField] float normDist = 0.5f;
		[SerializeField] string zAxis = "Mouse ScrollWheel";
		[SerializeField] int ignoreLayer = 8;		

		private Ray rayLeft;
		private Ray rayRight;
		private Ray rayTop;
		private Ray rayBottom;
		private RaycastHit hit;
		private List<Collider> others = new List<Collider>();

		void Awake()
		{
			if(target == null) enabled = false;
			destine = max;
		}

		void FixedUpdate()
		{
			Vector3 best = Vector3.back * destine;
			
			if(others.Count > 0){
				rayLeft.origin = target.position;
				rayRight.origin = target.position;
				rayTop.origin = target.position;
				rayBottom.origin = target.position;
				
				Vector3 dir = -transform.forward * destine;
				
				rayLeft.direction = dir + transform.right * -normDist;
				rayRight.direction = dir + transform.right * normDist;
				rayTop.direction = dir + transform.up * normDist;
				rayBottom.direction = dir + transform.up * -normDist;

#if UNITY_EDITOR
				Debug.DrawRay(rayLeft.origin, rayLeft.direction);
				Debug.DrawRay(rayRight.origin, rayRight.direction);
				Debug.DrawRay(rayTop.origin, rayTop.direction);
				Debug.DrawRay(rayBottom.origin, rayBottom.direction);
#endif

				foreach(Collider other in others)
				{
					if(other.Raycast(rayLeft, out hit, destine))
					{
						best = calcBest(hit, best);
					}
					if(other.Raycast(rayRight, out hit, destine))
					{
						best = calcBest(hit, best);
					}
					if(other.Raycast(rayTop, out hit, destine))
					{
						best = calcBest(hit, best);
					}
					if(other.Raycast(rayBottom, out hit, destine))
					{
						best = calcBest(hit, best);
					}
				}
			}
			transform.localPosition = best;
			
			//Change desire for next Update
			destine -= Input.GetAxisRaw(zAxis);
			destine = Mathf.Max(destine, min);
			destine = Mathf.Min(destine, max);
		}
		
		Vector3 calcBest(RaycastHit hit, Vector3 old)
		{
			Vector3 test = 	Vector3.back * hit.distance + 
							Vector3.right * Vector3.Dot(hit.normal, transform.right) * normDist + 
							Vector3.up * Vector3.Dot(hit.normal, transform.up) * normDist;
			return test.sqrMagnitude < old.sqrMagnitude ? test : old;
		}
		
		void OnTriggerEnter(Collider other)
		{
			if(other.isTrigger)
			{
				//ignore
			}
			else if(other.gameObject.layer == ignoreLayer) 
			{
				//ignore
			}
			else if(others.Contains(other))
			{
				//ignore
			}
			else
			{
				others.Add(other);
			}
		}

		void OnTriggerExit(Collider other)
		{
			others.Remove(other);
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