using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GB.EventSystems
{
    public sealed class GB_MessageEmitter : MonoBehaviour
    {
        [Serializable]
        public struct Entry
        {
            public string message;
            public UnityEvent callback;
        }

        [SerializeField] Entry[] actions = new Entry[0];

        readonly Dictionary<string, UnityEvent> MESSAGE_REGISTER = new Dictionary<string, UnityEvent>();

        void Start()
        {
            foreach(var entry in actions)
            {
                if(MESSAGE_REGISTER.ContainsKey(entry.message))
                {
#if UNITY_EDITOR
                    Debug.LogWarning("There is already a trigger on " + entry.message);
#endif
				}
                else
                {
                    MESSAGE_REGISTER.Add(entry.message, entry.callback);
                }
            }
        }

        public void EmitMessage(string message)
		{
			if (isActiveAndEnabled)
			{
				UnityEvent trigger;
				if(MESSAGE_REGISTER.TryGetValue(message, out trigger))
				{
					trigger.Invoke();
				}
			}
        }
    }
}
