using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Events
{
    public class GameEvent
    {
        public readonly string eventName;
        public UnityAction EventHandler;

        public GameEvent(string eventName)
        {
            this.eventName = eventName;
        }

        public void RaiseEvent()
        {
            if (EventHandler != null)
            {
                EventHandler.Invoke();
            } else
            {
                Debug.LogWarning($"A {eventName} was raised, but no handlers are listening.");
            }
        }
    }

    public class GameEvent<T>
    {
        public readonly string eventName;
        public UnityAction<T> EventHandler;

        public GameEvent(string eventName)
        {
            this.eventName = eventName;
        }

        public void RaiseEvent(T arg0)
        {
            if (EventHandler != null)
            {
                EventHandler.Invoke(arg0);
            }
            else
            {
                Debug.LogWarning($"A {eventName} event was raised, but no handlers are listening.");
            }
        }
    }
    
    public class GameEvent<T1, T2>
    {
        public readonly string eventName;
        public UnityAction<T1, T2> EventHandler;

        public GameEvent(string eventName)
        {
            this.eventName = eventName;
        }

        public void RaiseEvent(T1 arg0, T2 arg1)
        {
            if (EventHandler != null)
            {
                EventHandler.Invoke(arg0, arg1);
            }
            else
            {
                Debug.LogWarning($"A {eventName} event was raised, but no handlers are listening.");
            }
        }
    }
    
    public class GameEvent<T1, T2, T3>
    {
        public readonly string eventName;
        public UnityAction<T1, T2, T3> EventHandler;

        public GameEvent(string eventName)
        {
            this.eventName = eventName;
        }

        public void RaiseEvent(T1 arg0, T2 arg1, T3 arg2)
        {
            if (EventHandler != null)
            {
                EventHandler.Invoke(arg0, arg1, arg2);
            }
            else
            {
                Debug.LogWarning($"A {eventName} event was raised, but no handlers are listening.");
            }
        }
    }
    
    public class GameEvent<T1, T2, T3, T4>
    {
        public readonly string eventName;
        public UnityAction<T1, T2, T3, T4> EventHandler;

        public GameEvent(string eventName)
        {
            this.eventName = eventName;
        }

        public void RaiseEvent(T1 arg0, T2 arg1, T3 arg2, T4 arg3)
        {
            if (EventHandler != null)
            {
                EventHandler.Invoke(arg0, arg1, arg2, arg3);
            }
            else
            {
                Debug.LogWarning($"A {eventName} event was raised, but no handlers are listening.");
            }
        }
    }
}