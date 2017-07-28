using UnityEngine;

namespace GB.Character.RPG
{
	public abstract class GB_RPGAttribute : MonoBehaviour
	{
		[Header("Properties")]
		[SerializeField] float m_Max = 100;
		[SerializeField] float m_Curr = 100;
		[SerializeField] float m_Buff = 0;
		
		public float max { get { return m_Max; } protected set { m_Max = Mathf.Max(value, 1); } }
		public float curr { get { return m_Curr; } protected set { m_Curr = Mathf.Max(Mathf.Min(value, max), 0); } }
		public float buff { get { return m_Buff; } set { m_Buff = value; } }

		void Start()
		{
			curr = m_Curr;
			max = m_Max;
			ExtendedStart();
		}

		protected abstract void ExtendedStart();
	}
}

