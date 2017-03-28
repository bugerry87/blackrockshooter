using UnityEngine;

namespace GB.UI
{
	public class GB_UINotch : MonoBehaviour {

		enum Direction
		{
			FORWARD,
			RIGHT,
			UP
		}

		[SerializeField] private string objectName = "";
		[SerializeField] private GameObject target = null;
		[SerializeField] private float maxDistance = 1000;
		[SerializeField] private Direction direction = Direction.FORWARD;
		[SerializeField] private int checkLayer = 1;
		[SerializeField] private bool raycast = true;
		
		private Ray ray = new Ray();
		private RaycastHit hit;
		private Vector3 pos;

		void Start () {
			if (target == null)
			{
				target = GameObject.Find(objectName);
			}
			enabled = target != null;
		}
	
		void Update () {
			int negate = maxDistance < 0 ? -1 : 1;
			ray.origin = target.transform.position;
			switch (direction)
			{
				case Direction.FORWARD:
					ray.direction = target.transform.forward * negate;
					break;
				case Direction.RIGHT:
					ray.direction = target.transform.right * negate;
					break;
				case Direction.UP:
					ray.direction = target.transform.up * negate;
					break;
				default:
					ray.direction = target.transform.forward * negate;
					break;
			}

			if (raycast && Physics.Raycast(ray, out hit, maxDistance * negate, checkLayer, QueryTriggerInteraction.Ignore))
			{
				pos = hit.point;
			}
			else
			{
				pos = target.transform.position + (ray.direction * maxDistance);
			}

			if (transform is RectTransform)
			{
				transform.position = Camera.main.WorldToScreenPoint(pos);
			}
			else
			{
				transform.position = pos;
			}
		}
	}
}

