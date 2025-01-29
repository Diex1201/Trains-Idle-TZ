using UnityEngine;

public class MineNode : Node
{
    [SerializeField, Range(0.2f, 1f)] private float miningTimeMultiplier;
    public float MiningTimeMultiplier => miningTimeMultiplier;
}
public class MineData
{
    public Node Node;
    public float Distance;
    public float MiningTimeMultiplier;

    public MineData(Node node, float distance, float miningTimeMultiplier)
	{
        Node = node;
        Distance = distance;
        MiningTimeMultiplier = miningTimeMultiplier;
	}
}
