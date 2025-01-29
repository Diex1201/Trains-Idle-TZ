using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	[SerializeField] private NodeType nodeType;
	[SerializeField] private List<Neighbour> neighbours;
	public NodeType NodeType => nodeType;
	public List<Neighbour> Neighbours => neighbours;
}
public enum NodeType
{
	Base,
	Mine,
	Empty
}
