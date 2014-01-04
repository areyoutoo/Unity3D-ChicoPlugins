using UnityEngine;
using System.Collections;

/// <summary>
/// Extension methods for Unity's Vector3 class.
/// </summary>
public static class Vector3Extensions {
	public static Vector3 WithX(this Vector3 v, float x) {
		return new Vector3(x, v.y, v.z);
	}
	
	public static Vector3 WithY(this Vector3 v, float y) {
		return new Vector3(v.x, y, v.z);
	}
	
	public static Vector3 WithZ(this Vector3 v, float z) {
		return new Vector3(v.x, v.y, z);
	}
	
	public static Vector3 WithScale(this Vector3 v, Vector3 other) {
		return Vector3.Scale(v, other);
	}
	
	public static Vector3 WithLength(this Vector3 v, float length) {
		return v.normalized * length;
	}
	
	public static Vector2 ToVector2(this Vector3 v) {
		return new Vector2(v.x, v.y);
	}
}
