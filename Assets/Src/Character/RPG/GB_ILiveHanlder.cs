using UnityEngine.EventSystems;

namespace GBAssets.Character.RPG
{
    public interface GB_ILiveHandler : IEventSystemHandler
    {
        void TakeDemage(float demage);

		void Heal(float heal);

		void Recover();
    }
}

