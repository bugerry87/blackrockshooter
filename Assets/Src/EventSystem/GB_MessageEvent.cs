using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GBAssets.EventSystems
{
    public class GB_MessageEvent : MonoBehaviour, IEventSystemHandler, ISelectHandler, IDeselectHandler, GB_IMessageHandler
    {
        [Serializable]
        public class MessageTrigger : UnityEvent<object>
        {
            public MessageTrigger() : base() { }
        }

        [Serializable]
        public class Entry
        {
            [SerializeField] public string message;
            [SerializeField] public MessageTrigger callback;
        }

        [SerializeField] Entry[] trigger = null;

        public void OnSelect(BaseEventData data)
        {
            Debug.Log("Selected");
        }

        public void OnDeselect(BaseEventData data)
        {
            Debug.Log("Deselected");
        }

        public void OnMessage(object data)
        {
            foreach(Entry entry in trigger)
            {
                if(entry.message == data.ToString() && entry.callback != null)
                {
                    entry.callback.Invoke(data);
                }
            }
        }
    }
}
