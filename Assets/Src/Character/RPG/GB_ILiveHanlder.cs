using UnityEngine.EventSystems;

namespace GB.Character.RPG
{
    public interface GB_ILiveHandler : IEventSystemHandler
    {
        void TakeDemage(string type, float demage);

		void Heal(float heal);

		void Recover();
    }
}

