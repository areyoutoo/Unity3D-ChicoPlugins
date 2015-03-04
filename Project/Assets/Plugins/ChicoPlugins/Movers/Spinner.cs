using UnityEngine;
using System.Collections;

/// <summary>
/// Simple movement: rotates an object once per frame.
/// </summary>
[AddComponentMenu("ChicoPlugins/Movers/Spinner")]
public class Spinner : MonoBehaviour {

	public Vector3 axis = Vector3.up;
	public float degPerSecond = 90f;
	
	public bool local = true;
	
	void Update () {
		transform.Rotate(axis.normalized * Time.deltaTime * degPerSecond, local ? Space.Self : Space.World);
	}
}
