using System;
using UnityEngine;

namespace GBAssets.Utils
{
	public class GB_SimpleFollower : GB_AdoptedTarget
	{
		enum UpdateType {FixedUpdate, Update, LateUpdate}

		/*
		static Vector3 CalcPosition(Vector3 follower, Vector3 target, Vector3 factor)
		{
			return target - (Vector3.Scale(target - follower, factor));
		}
		*/

		[SerializeField] UpdateType updateType = UpdateType.FixedUpdate;

		void FixedUpdate()
		{
			if(updateType == UpdateType.FixedUpdate)
			{
				DoUpdate();
			}
		}

		void Update()
		{
			if(updateType == UpdateType.Update)
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

		void DoUpdate()
		{
			transform.position = adopted.position;
		}
	}
}