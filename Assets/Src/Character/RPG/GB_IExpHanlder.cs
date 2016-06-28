using UnityEngine.EventSystems;

namespace GBAssets.Character.RPG
{
    public interface GB_IExpHandler : IEventSystemHandler
    {
        void AddExp(int exp);
    }
}

