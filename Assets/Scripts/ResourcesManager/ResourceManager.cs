using System;
using UnityEngine;
using Zenject;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private int resources = 0;
    private EventBus eventBus;

    [Inject]
    public void Construct(EventBus eventBus)
	{
        this.eventBus = eventBus;
    }
    private void Awake()
    {
        eventBus.Subscribe<Events.ResourcesAddedEvent>(OnResourcesAdded);
    }
    private void OnDestroy()
    {
        eventBus.Unsubscribe<Events.ResourcesAddedEvent>(OnResourcesAdded);
    }
    private void OnResourcesAdded(Events.ResourcesAddedEvent e)
    {
        resources += e.Amount;
        eventBus.Publish(new Events.ResourcesChangedEvent { TotalResources = resources });
    }
    public int GetResources() => resources;
}
