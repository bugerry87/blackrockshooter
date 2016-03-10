using UnityEngine;

namespace GBAssets.Utils
{
	public class GB_SimpleFollower : GB_AdoptedTarget
	{
		[SerializeField] UpdateType updateType = UpdateType.FixedUpdate;

        void Update()
		{
			if(updateType == UpdateType.Update)
			{
				DoUpdate();
			}
		}

		void FixedUpdate()
		{
			if(updateType == UpdateType.FixedUpdate)
			{
				DoUpdate();
			}
		}

		void LateUpdate()
		{
			if(updateType == UpdateType.LateUpdate)
			{
				DoUpdate();
			}
		}

        public void ManualUpdate()
		{
			if(updateType == UpdateType.ManualUpdate)
			{
				DoUpdate();
			}
		}

		void DoUpdate()
		{
			transform.position = adopted.position;
		}
	}
}