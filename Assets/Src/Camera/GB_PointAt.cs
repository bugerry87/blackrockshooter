using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace GBAssets.CameraControl
{
	public class GB_PointAt : MonoBehaviour
	{
		[SerializeField]
		Camera cam = null;

		void Awake()
		{
			if (cam == null && Camera.main != null)
			{
				cam = Camera.main;
			}
			else
			{
#if UNITY_EDITOR
				Debug.Log("Camera missing!");
#endif
				enabled = false;
			}
		}

		void LateUpdate()
		{
			Ray ray = cam.ScreenPointToRay(CrossPlatformInputManager.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100, 1))
			{
				transform.LookAt(hit.point);
			}
			else
			{
				transform.forward = ray.direction;
			}
		}
	}
}
