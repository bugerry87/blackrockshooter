using UnityEngine;

namespace GBAssets.Utils
{

	public class GB_AdoptedTarget : MonoBehaviour 
	{

		[SerializeField] Transform m_target = null;
		[SerializeField] string m_searchTag = null;

		public Transform adopted { get{ return m_target; } set{ this.m_target = value; } }
		public string searchTag { get{ return m_searchTag; } private set{ this.m_searchTag = value; } }

		public void SetSearchTag(string tag)
		{
			if(tag != null)
			{
				this.enabled = true;
			}
			searchTag = tag;
		}

		bool IsTarget(GameObject t)
		{
			return searchTag != null && t.tag == searchTag;
		}

		void Awake ()
		{
			if (searchTag == null) this.enabled = false;
		}

		void OnTriggerStay(Collider other)
		{
			if(!IsTarget(other.gameObject))
			{
				//escape
			} 
			else if(adopted != null)
			{
				float dist1 = (other.transform.position - this.transform.position).magnitude;
				float dist2 = (adopted.position - this.transform.position).magnitude;
				if(dist1 < dist2)
				{
					this.adopted = other.transform;
				}
			}
			else
			{
				this.adopted = other.transform;
			}
		}

		void OnTriggerExit(Collider other)
		{
			if(IsTarget(other.gameObject))
			{
				this.adopted = null;
			}
		}
	}
}