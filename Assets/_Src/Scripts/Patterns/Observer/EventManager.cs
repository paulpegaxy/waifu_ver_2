using System;
using System.Collections.Generic;
using Doozy.Runtime.Signals;
using UnityEngine.Events;

namespace Template.Patterns
{
    public class EventManager
    {
        static readonly Dictionary<string, UnityEvent<object>> _events = new();
        static readonly Dictionary<string, object> _eventData = new();

        public static void StartListening(Enum eventName, UnityAction<object> callback)
        {
            string name = GetName(eventName);
            if (_events.TryGetValue(name, out UnityEvent<object> thisEvent))
            {
                thisEvent.AddListener(callback);
            }
            else
            {
                thisEvent = new UnityEvent<object>();
                thisEvent.AddListener(callback);

                _events.Add(name, thisEvent);
            }
        }

        public static void StopListening(Enum eventName, UnityAction<object> callback)
        {
            string name = GetName(eventName);
            if (_events.TryGetValue(name, out UnityEvent<object> thisEvent))
            {
                thisEvent.RemoveListener(callback);
            }
        }

        public static void EmitEvent(Enum eventName, object data = null)
        {
            string name = GetName(eventName);
            if (_eventData.ContainsKey(name))
            {
                _eventData[name] = data;
            }
            else
            {
                _eventData.Add(name, data);
            }

            if (_events.TryGetValue(name, out UnityEvent<object> thisEvent))
            {
                thisEvent.Invoke(data);
            }
        }

        public static void EmitEventWithSignal(Enum eventName, object data = null)
        {
            EmitEvent(eventName, data);
            // SignalsService.SendSignal(nameof(StreamId.Game), eventName.ToString());
        }

        public static T GetData<T>(Enum eventName, bool isRemove = false)
        {
            string name = GetName(eventName);
            if (_eventData.TryGetValue(name, out object data))
            {
                if (isRemove)
                {
                    _eventData.Remove(name);
                }
                return (T)data;
            }
            return default;
        }

        static string GetName(Enum eventName)
        {
            return eventName.GetType().Name + eventName.ToString();
        }
    }
}