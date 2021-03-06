﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Simple movement: bounces an object up and down.
/// </summary>
[AddComponentMenu("ChicoPlugins/Movers/Bouncer")]
public class Bouncer : MonoBehaviour {

	Vector3 origin;
	
	public Vector3 dir = Vector3.up;
	public float period = 4f;
	
	void Start () {
		origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float sin = Mathf.Sin(Time.time / (period * 2f * Mathf.PI));
		transform.position = origin + dir * sin;
	}
}
