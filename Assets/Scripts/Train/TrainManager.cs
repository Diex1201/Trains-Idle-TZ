using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class TrainManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> trainPrefabs;
	private Graph graph;
	private DiContainer container;

	[Inject]
	public void Construct(Graph graph, DiContainer container)
	{
		this.graph = graph;
		this.container = container;
		SpawnTrains();
	}

	private void SpawnTrains()
	{
		foreach (var train in trainPrefabs)
		{
			Node spawnNode = graph.GetSpawnNode();
			Train newTrain = container.InstantiatePrefab(train).GetComponent<Train>();
			newTrain.StartTrainBehaviour(spawnNode);
		}
	}
}
