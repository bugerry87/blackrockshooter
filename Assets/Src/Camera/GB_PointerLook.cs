using UnityEngine;
using GBAssets.Utils;

namespace GBAssets.CameraControl
{
    public enum RotationAxes
    {
        UseXAndY = 0,
        UseX = 1,
        UseY = 2
    }

	public class GB_PointerLook : GB_AUpdateMode
    {
        [SerializeField] RotationAxes axes = RotationAxes.UseXAndY;
		[SerializeField] GB_AdoptedTarget target = null;
		[SerializeField] float sensitivityX = 15F;
		[SerializeField] float sensitivityY = 15F;
		
		[SerializeField] float minimumY = -60F;
		[SerializeField] float maximumY = 60F;
		
		[SerializeField] string Xaxis = "X";
		[SerializeField] string Yaxis = "Y";
        [SerializeField] string disableButton = "Fire2";

		private float rotationX = 0F;
		private float rotationY = 0F;

        void Start ()
		{
			// Make the rigid body not change rotation
			if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;
            if(updateType == UpdateType.ManualUpdate)
            {
                enabled = false;
            }
		}

        protected override void DoUpdate(float deltaTime)
        {
            /*
            if(target && target.adopted)
			{
				transform.forward = target.adopted.position - transform.position;
			}
            */

            if (target && target.adopted || disableButton.Length != 0 && (Input.GetButton(disableButton) || Mathf.Abs(Input.GetAxis(disableButton)) > 0))
			{
				//DoNothing
			}
			else if (axes == RotationAxes.UseXAndY)
			{
				rotationX = transform.localEulerAngles.y + Input.GetAxis(Xaxis) * sensitivityX;
				
				rotationY += Input.GetAxis(Yaxis) * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.UseX)
			{
				transform.Rotate(0, Input.GetAxis(Xaxis) * sensitivityX, 0);
			}
			else
			{
				rotationY += Input.GetAxis(Yaxis) * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
        }
    }
}
