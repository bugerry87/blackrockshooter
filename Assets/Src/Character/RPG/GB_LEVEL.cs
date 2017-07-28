using UnityEngine;
using GB.EventSystems;

namespace GB.Character.RPG
{
	public class GB_LEVEL : GB_RPGAttribute
	{
		[SerializeField][Range(0.001f, 1f)] float scale = 0.125f;
		[SerializeField] protected GB_NamedFloatEvent emitLevelUp;
		[SerializeField] protected GB_NamedFloatEvent emitPercentage;

		public int exp { get; protected set; }

		protected override void ExtendedStart()
		{
			exp = (int) Mathf.Pow((int) curr / scale, 2);
		}

		public void AddExp(int exp)
		{
			this.exp += exp;
			if(exp >= NextLevel())
			{
				emitLevelUp.Invoke("Level", curr + 1);
			}
			curr = Mathf.Sqrt(this.exp) * scale;
			emitPercentage.Invoke("Percentage", Percentage());
		}

		public float NextLevel()
		{
			return Mathf.Pow((int) curr + 1 / scale, 2);
		}

		public float Percentage()
		{
			return curr - (int) curr;
		}
	}
}

