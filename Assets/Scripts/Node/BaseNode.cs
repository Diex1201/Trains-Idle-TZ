using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode : Node
{
	[SerializeField, Range(1,5) ] private float resourcesMultiplier;
	public float ResourcesMultiplier => resourcesMultiplier;
}
