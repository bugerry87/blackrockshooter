using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace GBAssets.Utils
{
	[RequireComponent(typeof(Camera))]
	public class GB_CamCollidePreventer : MonoBehaviour
	{
		[SerializeField] Transform target = null; 
		[SerializeField] float min = 1.0f;
		[SerializeField] float max = 10.0f;
		//[SerializeField] float beside = 0f;
		[SerializeField] float normDist = 0.5f;
		[SerializeField] string zAxis = "Mouse ScrollWheel";
		[SerializeField] int ignoreLayer = 8;
		
		private float desired;
		private Ray rayLeft;
		private Ray rayRight;
		private Ray rayTop;
		private Ray rayBottom;
		private RaycastHit hit;
		private List<Collider> others = new List<Collider>();

		void Awake()
		{
			if(target == null) this.enabled = false;
			desired = max;
		}

		void FixedUpdate()
		{
			Vector3 best = Vector3.back * desired;
			
			if(others.Count > 0){
				rayLeft.origin = target.position;
				rayRight.origin = target.position;
				rayTop.origin = target.position;
				rayBottom.origin = target.position;
				
				Vector3 dir = -transform.forward * desired;
				
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
					if(other.Raycast(rayLeft, out hit, desired))
					{
						best = calcBest(hit, best);
					}
					if(other.Raycast(rayRight, out hit, desired))
					{
						best = calcBest(hit, best);
					}
					if(other.Raycast(rayTop, out hit, desired))
					{
						best = calcBest(hit, best);
					}
					if(other.Raycast(rayBottom, out hit, desired))
					{
						best = calcBest(hit, best);
					}
				}
			}
			transform.localPosition = best;
			
			//Change desire for next Update
			desired -= CrossPlatformInputManager.GetAxisRaw(zAxis);
			desired = Mathf.Max(desired, min);
			desired = Mathf.Min(desired, max);
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
	}
}