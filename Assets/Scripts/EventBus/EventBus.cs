using System;
using System.Collections.Generic;

public class EventBus
{
    private readonly Dictionary<Type, List<Action<object>>> eventHandlers = new();
    public void Subscribe<T>(Action<T> handler)
    {
        Type eventType = typeof(T);
        if (!eventHandlers.ContainsKey(eventType))
        {
            eventHandlers[eventType] = new();
        }
        eventHandlers[eventType].Add(obj => handler((T)obj));
    }
    public void Unsubscribe<T>(Action<T> handler)
    {
        Type eventType = typeof(T);
        if (eventHandlers.ContainsKey(eventType))
        {
            eventHandlers[eventType].Remove(obj => handler((T)obj));
        }
    }
    public void Publish<T>(T message)
    {
        Type eventType = typeof(T);
        if (eventHandlers.ContainsKey(eventType))
        {
            foreach (var handler in eventHandlers[eventType])
            {
                handler(message);
            }
        }
    }
}
public static class Events
{
    public class ResourcesAddedEvent
    {
        public int Amount { get; set; }
    }
    public class ResourcesChangedEvent
    {
        public int TotalResources { get; set; }
    }
}
