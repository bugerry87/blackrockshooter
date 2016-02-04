using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GBAssets.EventSystems
{
    public class GB_ButtonEvent : MonoBehaviour, IEventSystemHandler, ISelectHandler, IDeselectHandler, GB_IButtonHandler
    {
        [Serializable]
        public class Entry
        {
            [SerializeField] public string buttonID;
            [SerializeField] public EventTrigger.TriggerEvent callback;
        }

        [SerializeField] Entry[] down = null;
        [SerializeField] Entry[] hold = null;
        [SerializeField] Entry[] up = null;

        public void OnButtonDown(GB_ButtonEventData data)
        {
            foreach(Entry entry in down)
            {
                if(entry.buttonID == data.name && entry.callback != null)
                {
                    entry.callback.Invoke(data);
                }
            }
        }

        public void OnButtonHold(GB_ButtonEventData data)
        {
            foreach(Entry entry in hold)
            {
                if(entry.buttonID == data.name && entry.callback != null)
                {
                    entry.callback.Invoke(data);
                }
            }
        }

        public void OnButtonUp(GB_ButtonEventData data)
        {
            foreach(Entry entry in up)
            {
                if(entry.buttonID == data.name && entry.callback != null)
                {
                    entry.callback.Invoke(data);
                }
            }
        }

        public void OnSelect(BaseEventData data)
        {
            Debug.Log("Selected");
        }

        public void OnDeselect(BaseEventData data)
        {
            Debug.Log("Deselected");
        }
    }
}
