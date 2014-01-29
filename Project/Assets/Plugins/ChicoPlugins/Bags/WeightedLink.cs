using UnityEngine;
using System.Collections;

/// <summary>
/// Weighted reference to some object. Mainly used by WeightedBag class.
/// </summary>
public class WeightedLink<T> {
	public readonly T target;
	public readonly float weight;
	
	public WeightedLink(T target, float weight) {
		if (weight < 0f) throw new System.ArgumentOutOfRangeException("weight");
		
		this.target = target;
		this.weight = weight;
	}
}
