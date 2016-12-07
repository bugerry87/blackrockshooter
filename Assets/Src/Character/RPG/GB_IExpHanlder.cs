using UnityEngine.EventSystems;

namespace GB.Character.RPG
{
    public interface GB_IExpHandler : IEventSystemHandler
    {
        void AddExp(int exp);
    }
}

