using UnityEngine;

namespace GBAssets.Utils
{

	public class GB_AdoptedTarget : MonoBehaviour 
	{

		[SerializeField] Transform m_target = null;
		[SerializeField] string m_searchTag = null;

		public Transform adopted { get{ return m_target; } set{ m_target = value; } }
		public string searchTag { get{ return m_searchTag; } private set{ this.m_searchTag = value; } }

		public void SetSearchTag(string tag)
		{
			if(tag != null)
			{
				enabled = true;
			}
			searchTag = tag;
		}

		bool IsTarget(GameObject t)
		{
			return searchTag != null && t.tag == searchTag;
		}

		void Awake ()
		{
			if (searchTag == null) enabled = false;
		}

		void OnTriggerStay(Collider other)
		{
			if(!IsTarget(other.gameObject))
			{
				//escape
			} 
			else if(adopted != null)
			{
				float dist1 = (other.transform.position - transform.position).magnitude;
				float dist2 = (adopted.position - transform.position).magnitude;
				if(dist1 < dist2)
				{
					adopted = other.transform;
				}
			}
			else
			{
				adopted = other.transform;
			}
		}

		void OnTriggerExit(Collider other)
		{
			if(IsTarget(other.gameObject))
			{
				adopted = null;
			}
		}
	}
}