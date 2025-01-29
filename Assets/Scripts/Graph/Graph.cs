using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
	[SerializeField] private List<Node> allNodes;
	public List<Node> AllNodes => allNodes;
	public Node GetSpawnNode()
	{
		return AllNodes[Random.Range(0, AllNodes.Count)];
	}
}
