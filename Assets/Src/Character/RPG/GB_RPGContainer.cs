using UnityEngine;
using System.Collections.Generic;

namespace GB.Character.RPG
{
	public class GB_RPGContainer : MonoBehaviour
	{
		protected readonly HashSet<GB_RPGAttribute> ATTRIBUTES = new HashSet<GB_RPGAttribute>();

		public bool AddAttribute(GB_RPGAttribute attribute)
		{
			return ATTRIBUTES.Add(attribute);
		}

		public bool RemoveAttribute(GB_RPGAttribute attribute)
		{
			return ATTRIBUTES.Remove(attribute);
		}

		T[] GetAttribute<T>() where T : GB_RPGAttribute
		{
			List<T> attributes = new List<T>();
			foreach(T attribute in ATTRIBUTES)
			{
				attributes.Add(attribute);
			}
			return attributes.ToArray();
		}
	}

}
