using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TrainVisual : MonoBehaviour
{
    [SerializeField] private GameObject trailPrefab;
    [SerializeField] private Material trailMaterial;
    private Train train => GetComponent<Train>();
    private GameObject currentTrail;
    private TrailPool trailPool;

    [Inject]
    public void Construc(TrailPool trailPool)
	{
        this.trailPool = trailPool;
	}
    
    private void Awake()
    {
        if (train != null)
        {
           train.OnMoveTo += DrawTrail;
           train.OnFinishedMove += DestroyTrail;
        }
    }
    private void OnDestroy()
    {
        if (train != null)
        {
            train.OnMoveTo -= DrawTrail;
            train.OnFinishedMove -= DestroyTrail;
        }
    }
	private async void DrawTrail(List<Node> targetNodes)
	{
        if (targetNodes == null || targetNodes.Count == 0) return;
		currentTrail = await trailPool.GetTrail();
		if (currentTrail == null) return;
		currentTrail.transform.position = transform.position;
        TrailRenderer trailRenderer = currentTrail.GetComponentInChildren<TrailRenderer>();
        trailRenderer.material = trailMaterial;
        trailRenderer.Clear();
        StartCoroutine(MoveTrail(targetNodes));
	}
	private IEnumerator MoveTrail(List<Node> targetNodes)
    {
        if (targetNodes == null || targetNodes.Count == 0) yield break;
        foreach (var currentNode in targetNodes)
        {
            while (Vector3.Distance(currentTrail.transform.position, currentNode.transform.position) > 0.01f)
            {
                currentTrail.transform.position = Vector3.MoveTowards(currentTrail.transform.position, currentNode.transform.position, 200 * Time.deltaTime);
                yield return null;
            }
        }
    }
    private void DestroyTrail()
	{
        trailPool.ReturnTrail(currentTrail);
	}
}
