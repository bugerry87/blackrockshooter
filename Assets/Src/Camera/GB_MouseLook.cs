using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

using GBAssets.Utils;

namespace GBAssets.CameraControl
{
	public class GB_MouseLook : MonoBehaviour
    {
		enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 };

		[SerializeField] GB_AdoptedTarget target = null;
		[SerializeField] RotationAxes axes = RotationAxes.MouseXAndY;
		[SerializeField] float sensitivityX = 15F;
		[SerializeField] float sensitivityY = 15F;
		
		[SerializeField] float minimumY = -60F;
		[SerializeField] float maximumY = 60F;
		
		[SerializeField] string Xaxis = "Mouse X";
		[SerializeField] string Yaxis = "Mouse Y";
		[SerializeField] string disableButton = "Fire2";

		private float rotationX = 0F;
		private float rotationY = 0F;

		void LateUpdate ()
		{
			if (target && target.adopted || CrossPlatformInputManager.GetButton(disableButton))
			{
				//DoNothing
			}
			else if (axes == RotationAxes.MouseXAndY)
			{
				rotationX = transform.localEulerAngles.y + CrossPlatformInputManager.GetAxis(Xaxis) * sensitivityX;
				
				rotationY += CrossPlatformInputManager.GetAxis(Yaxis) * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, CrossPlatformInputManager.GetAxis(Xaxis) * sensitivityX, 0);
			}
			else
			{
				rotationY += CrossPlatformInputManager.GetAxis(Yaxis) * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}

		void FixedUpdate ()
		{
			if(target && target.adopted)
			{
				transform.forward = target.adopted.position - transform.position;
			}
		}
		
		void Start ()
		{
			// Make the rigid body not change rotation
			if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;
		}

    }
}
