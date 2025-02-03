using System;
using System.Collections.Generic;

public class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> eventHandlers = new();
    public void Subscribe<T>(Action<T> handler)
    {
        Type eventType = typeof(T); 
        if (!eventHandlers.ContainsKey(eventType)) 
        {
            eventHandlers[eventType] = new(); 
        }
        eventHandlers[eventType].Add(handler);
    }
    public void Unsubscribe<T>(Action<T> handler)
    {
        Type eventType = typeof(T); 
        if (eventHandlers.ContainsKey(eventType))
        {
            eventHandlers[eventType].Remove(handler);
        }
    }
    public void Publish<T>(T message)
    {
        Type eventType = typeof(T); 
        if (eventHandlers.ContainsKey(eventType))
        {
            foreach (var handler in eventHandlers[eventType])
            {
                if(handler is Action<T> action)
                action(message);
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
