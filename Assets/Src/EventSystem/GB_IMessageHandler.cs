using UnityEngine.EventSystems;

namespace GBAssets.EventSystems
{
    public interface GB_IMessageHandler : IEventSystemHandler
    {
        void OnMessage(object data);
    }
}