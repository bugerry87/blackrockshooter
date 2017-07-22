using UnityEngine;

namespace GB.Utils
{
	public class GB_Spawner : MonoBehaviour
	{
		public enum SpawnType { Auto, Trigger, Manuel }

		[Header("Prefabs")]
		[SerializeField] protected GameObject m_Prefab;
		[SerializeField] protected Transform m_Parent;
		[Header("Spawn")]
		[SerializeField] protected SpawnType m_SpawnType = SpawnType.Auto;
		[SerializeField] int m_Amount = 1;
		[SerializeField] protected float m_SpawnDelay;
		[SerializeField] protected float m_DelayRandom;
		[SerializeField] protected string m_TriggerTag;
		[Header("Self Destroy")]
		[SerializeField] protected bool m_DestroyWhenDone;
		[SerializeField] protected float m_DestroyDelay;

		public BoxCollider box { get; protected set; }
		public SphereCollider sphere { get; protected set; }
		
		public float timeout { get; protected set; }
		public int amount { get { return m_Amount; } set { m_Amount = value; } }
		
		void Start()
		{
			sphere = GetComponent<SphereCollider>();
			box = GetComponent<BoxCollider>();
			ResetTimeout();
		}
		
		void FixedUpdate()
		{
			if (m_SpawnType == SpawnType.Auto)
			{
				DoSpawn();
			}
		}

		void OnTriggerEnter(Collider other)
		{
			if(m_SpawnType == SpawnType.Trigger && (m_TriggerTag == null || m_TriggerTag == other.tag))
			{
				DoSpawn();
			}
		}

		public void Spawn()
		{
			if(m_SpawnType == SpawnType.Manuel)
			{
				DoSpawn();
			}
		}

		protected void DoSpawn()
		{
			if (m_Prefab == null)
			{
				//ignore
			}
			else if(m_Amount == -1 || amount > 0)
			{
				if(timeout < Time.time)
				{
					Vector3 pos = transform.position;
					if (sphere)
					{
						pos += sphere.center + new Vector3(Random.value, Random.value, Random.value) * sphere.radius;
					}
					else if (box)
					{
						pos += box.center + new Vector3(box.size.x * Random.value - box.size.x * 0.5f, box.size.y * Random.value - box.size.y * 0.5f, box.size.z * Random.value - box.size.z * 0.5f);
					}

					m_Prefab.transform.position = pos;
					m_Prefab.transform.rotation = transform.rotation;
					Instantiate(m_Prefab).transform.SetParent(m_Parent);
					ResetTimeout();
					if(m_Amount != -1) amount--;
				}
			}
			else if(m_DestroyWhenDone)
			{
				Destroy(gameObject, m_DestroyDelay);
			}
		}

		public void ResetTimeout()
		{
			timeout = Time.time + m_SpawnDelay + m_DelayRandom * Random.value;
		}
	}
}


