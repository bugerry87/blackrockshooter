using UnityEngine;

//obsolete!!!
namespace GB.CameraControl
{
	public class GB_PointAt : MonoBehaviour
	{
		[SerializeField] Camera cam = null;
        [SerializeField] bool useMousePointer = true;

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
			Ray ray = useMousePointer ? cam.ScreenPointToRay(Input.mousePosition) : cam.ScreenPointToRay(cam.pixelRect.size * 0.5f);
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
