using UnityEngine;
using System.Collections;

public class Facer : MonoBehaviour {
	public Transform target;
	public bool freezeVertical = true;

	// Update is called once per frame
	void Update () {
		Vector3 targetPos = target.position;
		if (freezeVertical) {
			targetPos.y = transform.position.y;
		}
		transform.LookAt(targetPos);
	}
}
