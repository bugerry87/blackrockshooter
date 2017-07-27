using System;
using UnityEngine;
using UnityEngine.Events;

namespace GB.EventSystems
{
	[Serializable]
	public class GB_NamedFloatEvent : UnityEvent<string, float>
	{
	}

	[Serializable]
	public class GB_VecEvent : UnityEvent<Vector3>
	{
	}

	[Serializable]
	public class GB_TransformEvent : UnityEvent<Transform>
	{
	}
}
