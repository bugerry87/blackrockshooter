using System;
using UnityEngine;
using UnityEngine.Events;

namespace GB.EventSystems
{
	[Serializable]
	public class GB_FloatEvent : UnityEvent<string, float>
	{
	}

	[Serializable]
	public class GB_VecEvent : UnityEvent<Vector3>
	{
	}
}
