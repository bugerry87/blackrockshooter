using System;
using UnityEngine;

namespace GB.Utils
{
    public enum UpdateType // The available methods of updating are:
    {
        Update,
        LateUpdate, // Update in LateUpdate. (for tracking objects that are moved in Update)
        FixedUpdate, // Update in FixedUpdate (for tracking rigidbodies).
        ManualUpdate // user must call to update camera
    }

    public abstract class GB_AUpdateMode : MonoBehaviour
    {
        [SerializeField] protected UpdateType updateType = UpdateType.FixedUpdate;

		public event Action OnUpdate;
		public event Action OnFixedUpdate;
		public event Action OnLateUpdate;

        void Update()
        {
            if(updateType == UpdateType.Update)
            {
                DoUpdate(Time.deltaTime);
            }
			if(OnUpdate != null) OnUpdate();
        }

        void FixedUpdate ()
		{
            if(updateType == UpdateType.FixedUpdate)
            {
                DoUpdate(Time.fixedDeltaTime);
            }
			if(OnFixedUpdate != null) OnFixedUpdate();
		}

		void LateUpdate ()
		{
			if(updateType == UpdateType.LateUpdate)
            {
                DoUpdate(Time.deltaTime);
            }
			if(OnLateUpdate != null) OnLateUpdate();
		}

        public void ManualUpdate(float deltaTime)
        {
            if(updateType == UpdateType.ManualUpdate)
            {
                DoUpdate(deltaTime);
            }
        }

        public void ManualUpdate(UpdateType like)
        {
            switch (like)
            {
                case UpdateType.Update:
                case UpdateType.LateUpdate: DoUpdate(Time.deltaTime); break;
                case UpdateType.FixedUpdate: DoUpdate(Time.fixedDeltaTime); break;
                default: Debug.LogError("Invalid UpdateType"); break;
            }
        }

        protected abstract void DoUpdate(float deltaTime);
    }
}
