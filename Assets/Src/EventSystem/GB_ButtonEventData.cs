﻿using UnityEngine.EventSystems;

namespace GBAssets.EventSystems
{
    public class GB_ButtonEventData : BaseEventData
    {
        public string name { get; private set; }

        public GB_ButtonEventData(EventSystem eventSystem, string name) : base (eventSystem)
        {
            this.name = name;
        }
    }
}
