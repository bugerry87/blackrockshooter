using System;
using System.Threading;
using UnityEngine;

namespace GBAssets.Utils
{
	public class GB_ActionScheduler : ScriptableObject
	{
		protected readonly Action<string, Action, int> locker;

		protected GB_ActionScheduler() : base()
		{
			locker = DoLock;
		}

		public void Request(string key, Action action, int cooldown = 0, AsyncCallback callback = null)
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

		public void Queue(string key, Action action, int cooldown = 0, AsyncCallback callback = null)
		{
			locker.BeginInvoke(key, action, cooldown, callback, null);
		}

		protected void DoLock(string key, Action action, int cooldown)
		{
			lock (key)
			{
				action();
				Thread.Sleep(cooldown);
			}
		}
	}
}
