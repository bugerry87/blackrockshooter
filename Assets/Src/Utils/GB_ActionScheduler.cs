using System;
using System.Threading;
using UnityEngine;

namespace GBAssets.Utils
{
	public class GB_ActionScheduler : ScriptableObject
	{
		protected static readonly Action<object, Action, int> locker = (object key, Action action, int cooldown) =>
		{
			lock (key)
			{
				if(action != null) action();
				Thread.Sleep(cooldown);
			}
		};

		public void Request(object key, Action action, int cooldown = 0, AsyncCallback callback = null)
		{
			if (Monitor.TryEnter(key))
			{
				try
				{
					Queue(key, action, cooldown, callback);
				}
				finally
				{
					Monitor.Exit(key);
				}
			}
		}

		public void Queue(object key, Action action, int cooldown = 0, AsyncCallback callback = null)
		{
			locker.BeginInvoke(key, action, cooldown, callback, null);
		}
	}
}
