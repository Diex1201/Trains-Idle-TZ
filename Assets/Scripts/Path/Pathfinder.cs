using System;
using System.Collections.Generic;
using UnityEngine;
public class PathNode
{
    public Node Node;
    public float Distance;
    public PathNode(Node node, float distance)
    {
        Node = node;
        Distance = distance;
    }
}
public class Pathfinder
{
    public List<Node> FindPathToMine(Node startNode, float trainSpeed, float trainMiningTime)
    {
        Func<float, Node, float> calculateMineEfficiency = (distance, node) =>
        {
            if (node.NodeType == NodeType.Mine)
            {
                MineNode mineNode = node as MineNode;
                return CalculateMineEfficiency(distance, mineNode.MiningTimeMultiplier, trainSpeed, trainMiningTime);
            }
            return float.NegativeInfinity;
        };
        return FindPath(startNode, trainSpeed, calculateMineEfficiency);
    }
    public List<Node> FindPathToBase(Node startNode, float trainSpeed)
	{
        Func<float, Node, float> calculateBaseEfficiency = (distance, node) =>
        {
            if (node.NodeType == NodeType.Base)
            {
                BaseNode baseNode = node as BaseNode;
                return CalculateBaseEfficiency(distance, baseNode.ResourcesMultiplier, trainSpeed);
            }
            return float.NegativeInfinity;
        };
        return FindPath(startNode, trainSpeed, calculateBaseEfficiency);
    }
    private float CalculateMineEfficiency(float distance, float miningTimeMultiplier, float trainSpeed, float trainMiningTime)
    {
        float travelTime = distance / trainSpeed;
        float totalTime = travelTime + trainMiningTime * miningTimeMultiplier;
        if (totalTime <= 0) return float.NegativeInfinity;
        return 1 / totalTime;
    }
    private float CalculateBaseEfficiency(float distance, float resourceMultiplier, float trainSpeed)
    {
        float travelTime = distance / trainSpeed;
        if (travelTime <= 0) return float.NegativeInfinity;
        return resourceMultiplier / travelTime;
    }
    private List<Node> ReconstructPath(Node targetNode, Dictionary<Node, Node> previousNodes)
    {
        List<Node> path = new();
        Node current = targetNode;
        while (current != null && previousNodes.ContainsKey(current))
        {
            path.Insert(0, current);
            current = previousNodes[current];
        }
        return path;
    }
    public List<Node> FindPath(Node startNode, float trainSpeed, Func<float, Node, float> calculateEfficiency)
	{
        Queue<PathNode> queue = new();
        HashSet<Node> visited = new();
        Dictionary<Node, float> distances = new();
        Dictionary<Node, Node> previousNodes = new();
        queue.Enqueue(new PathNode(startNode, 0));
        visited.Add(startNode);
        distances.Add(startNode, 0);
        Node bestNode = null;
        float maxEfficiency = float.NegativeInfinity;
        while (queue.Count > 0)
        {
            PathNode currentPathNode = queue.Dequeue();
            Node currentNode = currentPathNode.Node;
            float currentDistance = currentPathNode.Distance;

            float efficiency = calculateEfficiency(currentDistance, currentNode);

            if (efficiency > maxEfficiency)
            {
                maxEfficiency = efficiency;
                bestNode = currentNode;
            }

            foreach (Neighbour neighbour in currentNode.Neighbours)
            {
                if (!visited.Contains(neighbour.NeighbourNode))
                {
                    visited.Add(neighbour.NeighbourNode);
                    distances.Add(neighbour.NeighbourNode, currentDistance + neighbour.DistanceToNeighbourNode);
                    previousNodes.Add(neighbour.NeighbourNode, currentNode);
                    queue.Enqueue(new PathNode(neighbour.NeighbourNode, currentDistance + neighbour.DistanceToNeighbourNode));
                }
            }
        }
        if (bestNode != null)
        {
            return ReconstructPath(bestNode, previousNodes);
        }
        else
        {
            Debug.LogWarning("No target node found!");
            return null;
        }
    }
}
