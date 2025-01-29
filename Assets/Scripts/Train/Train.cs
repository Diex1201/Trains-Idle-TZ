using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class Train : MonoBehaviour
{
	[SerializeField, Range(5, 200)] private float moveSpeed;
	[SerializeField, Range(1, 20)] private float miningTime;
	[SerializeField] private List<Node> targetNodes = new();
	[SerializeField] private Node currentNode;
	[SerializeField] private float rotateSpeed = 90f;// необязательное поле, для скорости вращения, во время добычи ресурсов, просто ради визуала
	[SerializeField] private float extraMultiplierToSpeed; //необязательное поле, для корретикровки скорости движения поездов, потому что медленный ползет как улитка, а быстрый почти телепортируется визуально
	Pathfinder pathfinder;
	private EventBus eventBus;
	public event Action<List<Node>> OnMoveTo;
	public event Action OnFinishedMove;
	[Inject]
	public void Construct(Pathfinder pathfinder, EventBus eventBus)
	{
		this.pathfinder = pathfinder;
		this.eventBus = eventBus;
	}
	public void StartTrainBehaviour(Node spawnNode)
	{
		currentNode = spawnNode;
		transform.position = currentNode.transform.position;
        StartCoroutine(MoveTo(true));
	}
	private IEnumerator MoveTo(bool isMoveToMine)
	{
		targetNodes = new();
		if(isMoveToMine)
		{
			targetNodes = pathfinder.FindPathToMine(currentNode, moveSpeed, miningTime);
		}
		else
		{
			targetNodes = pathfinder.FindPathToBase(currentNode, moveSpeed);
		}

		OnMoveTo?.Invoke(targetNodes);

		foreach (var currentNode in targetNodes)
		{
			while (Vector3.Distance(transform.position, currentNode.transform.position) > 0.01)
			{
				transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, moveSpeed * extraMultiplierToSpeed * Time.deltaTime);
				transform.LookAt(currentNode.transform);
				yield return null;
			}
			this.currentNode = currentNode;
		}
		OnFinishedMove?.Invoke();
		if (currentNode.NodeType == NodeType.Mine) StartCoroutine(MineResources());
		else StartCoroutine(AddResourcesToBase());
	}
	private IEnumerator MineResources()
	{
		float currentMiningTime = miningTime;
		if(currentNode.NodeType == NodeType.Mine)
		{
			MineNode mineNode = currentNode as MineNode;
			currentMiningTime *= mineNode.MiningTimeMultiplier;
		}
		while(currentMiningTime > 0)
		{
			transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime); //просто визуал, показать что поезд добывает ресурсы
			currentMiningTime -= Time.deltaTime;
			yield return null;
		}
		StartCoroutine(MoveTo(false));
	}
	private IEnumerator AddResourcesToBase()
	{
		if (currentNode.NodeType == NodeType.Base)
		{
			BaseNode baseNode = currentNode as BaseNode;
			int resourcesToAdd = (int)(1 * baseNode.ResourcesMultiplier);
			eventBus.Publish(new Events.ResourcesAddedEvent { Amount = resourcesToAdd });
		}
		StartCoroutine(MoveTo(true));
		yield break;
	}
}

