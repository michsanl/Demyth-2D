using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomCode.Tools.EventManager
{
    public class GameEvent : GameEventHandler
    {
        public string eventName;
        public GameEvent(string eventName)
        {
            this.eventName = eventName;
        }
    }
}
