using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GBAssets.EventSystems
{
    public sealed class GB_MessageEvent : MonoBehaviour, GB_IMessageHandler
    {
        static Action<string> emitter = null;

        public static void Emit(string message)
        {
            if(emitter != null)
            {
                emitter(message);
            }
            else
            {
                Debug.LogWarning("No emitter registered!");
            }
        }

        [Serializable]
        public class MessageTrigger : UnityEvent
        {
            public MessageTrigger() : base() { }
        }

        [Serializable]
        public class Entry
        {
            [SerializeField] public string message;
            [SerializeField] public MessageTrigger callback;
        }

        [SerializeField] Entry[] actions = new Entry[0];

        readonly Dictionary<string, MessageTrigger> MESSAGE_REGISTER = new Dictionary<string, MessageTrigger>();

        void Start()
        {
            //register its self
            emitter += OnMessage;
            //register all entries
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

		void OnDestroy()
		{
			emitter -= OnMessage;
		}

        public void OnMessage(string message)
        {
			if (isActiveAndEnabled)
			{
				MessageTrigger trigger;
				if(MESSAGE_REGISTER.TryGetValue(message, out trigger))
				{
					trigger.Invoke();
				}
			}
        }		
    }
}
