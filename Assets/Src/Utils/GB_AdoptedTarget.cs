using UnityEngine;
using GB.EventSystems;

namespace GB.Utils
{

	public class GB_AdoptedTarget : MonoBehaviour 
	{
		[SerializeField] Transform m_target = null;
		[SerializeField] string m_searchTag = null;
		[SerializeField] GB_TransformEvent emitTarget;

		public Transform target { get{ return m_target; } set{ m_target = value; } }
		public string searchTag { get{ return m_searchTag; } private set{ m_searchTag = value; } }

		public void SetSearchTag(string tag)
		{
			if(tag != null)
			{
				enabled = true;
			}
			searchTag = tag;
		}

		public bool IsTarget(Transform t)
		{
			return searchTag != null && t && t.tag == searchTag;
		}

		protected virtual void Awake ()
		{
			if (searchTag == null) enabled = false;
		}

		protected virtual void OnTriggerStay(Collider other)
		{
			if(!IsTarget(other.transform))
			{
				//escape
			} 
			else if(target != null)
			{
				float dist1 = (other.transform.position - transform.position).sqrMagnitude;
				float dist2 = (target.position - transform.position).sqrMagnitude;
				if(dist1 < dist2 && target != other.transform)
				{
					target = other.transform;
					emitTarget.Invoke(target);
				}
			}
			else
			{
				target = other.transform;
				emitTarget.Invoke(target);
			}
		}
	}
}