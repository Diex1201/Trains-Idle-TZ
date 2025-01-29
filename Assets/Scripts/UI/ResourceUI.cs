using TMPro;
using Zenject;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resourceText;
    private EventBus eventBus;
    [Inject]
    public void Construct(EventBus eventBus)
    {
        this.eventBus = eventBus;
    }
    private void Awake()
    {
        eventBus.Subscribe<Events.ResourcesChangedEvent>(UpdateResourceText);
        UpdateResourceText(new Events.ResourcesChangedEvent() { TotalResources = 0 });
    }
    private void OnDestroy()
    {
        eventBus.Unsubscribe<Events.ResourcesChangedEvent>(UpdateResourceText);
    }
    private void UpdateResourceText(Events.ResourcesChangedEvent e)
    {
        resourceText.text = $"Resources: {e.TotalResources}";
    }
}
