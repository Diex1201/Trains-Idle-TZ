using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
	[SerializeField] GameObject graph;
	[SerializeField] GameObject resourcesManager;
	[SerializeField] GameObject trailPool;
	public override void InstallBindings()
	{
		Container.Bind<Graph>().FromInstance(graph.GetComponent<Graph>()).AsSingle();
		Container.Bind<ResourceManager>().FromInstance(resourcesManager.GetComponent<ResourceManager>()).AsSingle();
		Container.Bind<TrailPool>().FromInstance(trailPool.GetComponent<TrailPool>()).AsSingle();
		Container.Bind<Pathfinder>().AsSingle();
		Container.Bind<EventBus>().AsSingle();
	}
}
