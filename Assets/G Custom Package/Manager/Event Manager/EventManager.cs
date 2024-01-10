using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*http://www.willrmiller.com/?p=87*/
namespace CustomCode.Tools.EventManager
{
	public class GameEventHandler { }
    public static class EventManager
    {
		public delegate void EventDelegate<T>(T e) where T : GameEventHandler;
		private delegate void EventDelegate(GameEventHandler e);

		private static Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
		private static Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();

		public static void AddListener<T>(EventDelegate<T> del) where T : GameEventHandler
		{
			// Early-out if we've already registered this delegate
			if (delegateLookup.ContainsKey(del))
				return;

			// Create a new non-generic delegate which calls our generic one.
			// This is the delegate we actually invoke.
			EventDelegate internalDelegate = (e) => del((T)e);
			delegateLookup[del] = internalDelegate;

			EventDelegate tempDel;
			if (delegates.TryGetValue(typeof(T), out tempDel))
			{
				delegates[typeof(T)] = tempDel += internalDelegate;
			}
			else
			{
				delegates[typeof(T)] = internalDelegate;
			}
		}

		public static void RemoveListener<T>(EventDelegate<T> del) where T : GameEventHandler
		{
			EventDelegate internalDelegate;
			if (delegateLookup.TryGetValue(del, out internalDelegate))
			{
				EventDelegate tempDel;
				if (delegates.TryGetValue(typeof(T), out tempDel))
				{
					tempDel -= internalDelegate;
					if (tempDel == null)
					{
						delegates.Remove(typeof(T));
					}
					else
					{
						delegates[typeof(T)] = tempDel;
					}
				}

				delegateLookup.Remove(del);
			}
		}

		public static void Raise(GameEventHandler e)
		{
			EventDelegate del;
			if (delegates.TryGetValue(e.GetType(), out del))
			{
				del.Invoke(e);
			}
		}
	}
}
