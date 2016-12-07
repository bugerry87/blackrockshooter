using UnityEngine.EventSystems;

namespace GB.EventSystems
{
    public interface GB_IButtonHandler : IEventSystemHandler
    {
        void OnButtonDown(GB_ButtonEventData data);

        void OnButtonHold(GB_ButtonEventData data);

        void OnButtonUp(GB_ButtonEventData data);
    }
}

