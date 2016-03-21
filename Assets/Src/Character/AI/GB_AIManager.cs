using System;
using UnityEngine;
using GBAssets.Utils;

namespace GBAssets.Character.AI
{
	public class GB_AIManager : ScriptableObject
	{
		public const string ATTACK = "ATTACK";

		public void Request(GB_AI ai, string action)
		{
			switch (action)
			{
				case ATTACK: break;
				default: break;
			}
		}


	}
}
