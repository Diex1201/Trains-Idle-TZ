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
        Queue<PathNode> queue = new();
        HashSet<Node> visited = new();
        Dictionary<Node, float> distances = new();
        Dictionary<Node, Node> previousNodes = new();
        queue.Enqueue(new PathNode(startNode, 0));
        visited.Add(startNode);
        distances.Add(startNode, 0);

        Node bestMine = null;
        float maxEfficiency = float.MinValue;

        while (queue.Count > 0)
        {
            PathNode currentPathNode = queue.Dequeue();
            Node current = currentPathNode.Node;
            float currentDistance = currentPathNode.Distance;

            if (current.NodeType == NodeType.Mine)
            {
                MineNode mineNode = current as MineNode;
                float miningTimeMultiplier = mineNode.MiningTimeMultiplier;
                float efficiency = CalculateMineEfficiency(currentDistance, miningTimeMultiplier, trainSpeed, trainMiningTime);
                if (bestMine == null)
                {
                    maxEfficiency = efficiency;
                    bestMine = current;
                }
                else if (!float.IsNaN(efficiency) && !float.IsInfinity(efficiency) && efficiency > maxEfficiency)
                {
                    maxEfficiency = efficiency;
                    bestMine = current;
                }
                continue;
            }
            foreach (Neighbour neighbour in current.Neighbours)
            {
                if (!visited.Contains(neighbour.NeighbourNode))
                {
                    visited.Add(neighbour.NeighbourNode);
                    distances.Add(neighbour.NeighbourNode, currentDistance + neighbour.DistanceToNeighbourNode);
                    previousNodes.Add(neighbour.NeighbourNode, current);
                    queue.Enqueue(new PathNode(neighbour.NeighbourNode, currentDistance + neighbour.DistanceToNeighbourNode));
                }
            }
        }
        if (bestMine != null)
        {
            return ReconstructPath(bestMine, previousNodes);
        }
        else
        {
            Debug.LogWarning("No mine found!");
            return null;
        }
    }
    public List<Node> FindPathToBase(Node startNode, float trainSpeed)
	{
        Queue<PathNode> queue = new();
        HashSet<Node> visited = new();
        Dictionary<Node, float> distances = new();
        Dictionary<Node, Node> previousNodes = new();
        queue.Enqueue(new PathNode(startNode, 0));
        visited.Add(startNode);
        distances.Add(startNode, 0);
        Node bestBase = null;
        float maxEfficiency = float.MinValue;
        while (queue.Count > 0)
        {
            PathNode currentPathNode = queue.Dequeue();
            Node current = currentPathNode.Node;
            float currentDistance = currentPathNode.Distance;
            if (current.NodeType == NodeType.Base)
            {
                BaseNode baseNode = current as BaseNode;
                float efficiency = CalculateBaseEfficiency(currentDistance, baseNode.ResourcesMultiplier, trainSpeed);
                if (bestBase == null)
                {
                    maxEfficiency = efficiency;
                    bestBase = current;
                }
                else if (!float.IsNaN(efficiency) && !float.IsInfinity(efficiency) && efficiency > maxEfficiency)
                {
                    maxEfficiency = efficiency;
                    bestBase = current;
                }
                continue;
            }
            foreach (Neighbour neighbour in current.Neighbours)
            {
                if (!visited.Contains(neighbour.NeighbourNode))
                {
                    visited.Add(neighbour.NeighbourNode);
                    distances.Add(neighbour.NeighbourNode, currentDistance + neighbour.DistanceToNeighbourNode);
                    previousNodes.Add(neighbour.NeighbourNode, current);
                    queue.Enqueue(new PathNode(neighbour.NeighbourNode, currentDistance + neighbour.DistanceToNeighbourNode));
                }
            }
        }
        if (bestBase != null)
        {
            return ReconstructPath(bestBase, previousNodes);
        }
        else
        {
            Debug.LogWarning("No base found!");
            return null;
        }
    }
    private float CalculateMineEfficiency(float distance, float miningTimeMultiplier, float trainSpeed, float trainMiningTime)
    {
        float travelTime = distance / trainSpeed;
        float totalTime = travelTime + trainMiningTime * miningTimeMultiplier;
        if (totalTime == 0) return 0;
        if (travelTime == 0 || trainMiningTime == 0) return 0;
        return 1 / totalTime;
    }
    private float CalculateBaseEfficiency(float distance, float resourceMultiplier, float trainSpeed)
    {
        float travelTime = distance / trainSpeed;
        return resourceMultiplier / travelTime;
    }
    private List<Node> ReconstructPath(Node targetNode, Dictionary<Node, Node> previousNodes)
    {
        List<Node> path = new List<Node>();
        Node current = targetNode;
        while (current != null && previousNodes.ContainsKey(current))
        {
            path.Insert(0, current);
            current = previousNodes[current];
        }
        return path;
    }
}
