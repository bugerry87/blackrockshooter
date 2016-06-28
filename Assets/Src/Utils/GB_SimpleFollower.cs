using UnityEngine;

namespace GBAssets.Utils
{
	public class GB_SimpleFollower : GB_AdoptedTarget
	{
		[SerializeField] UpdateType updateType = UpdateType.FixedUpdate;
		[SerializeField] float maxSpeed = 1f;

        void Update()
		{
			if(updateType == UpdateType.Update)
			{
				DoUpdate(Time.deltaTime);
			}
		}

		void FixedUpdate()
		{
			if(updateType == UpdateType.FixedUpdate)
			{
				DoUpdate(Time.fixedDeltaTime);
			}
		}

		void LateUpdate()
		{
			if(updateType == UpdateType.LateUpdate)
			{
				DoUpdate(Time.deltaTime);
			}
		}

        public void ManualUpdate(float delta)
		{
			if(updateType == UpdateType.ManualUpdate)
			{
				DoUpdate(delta);
			}
		}

		void DoUpdate(float delta)
		{
			transform.position = Vector3.Lerp(transform.position, target.position, maxSpeed * delta);
			transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, maxSpeed * delta);
		}
	}
}