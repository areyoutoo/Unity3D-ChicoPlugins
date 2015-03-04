using UnityEngine;
using System.Collections;

/// <summary>
/// Extension methods for Unity's Vector2 class.
/// </summary>
public static class Vector2Extensions {
	public static Vector3 ToVector3(this Vector2 v, float z) {
		return new Vector3(v.x, v.y, z);
	}
	
	public static Vector3 WithX(this Vector2 v, float x) {
		return new Vector2(x, v.y);
	}
	
	public static Vector3 WithY(this Vector2 v, float y) {
		return new Vector2(v.x, y);
	}
	
	public static Vector2 WithLength(this Vector2 v, float length) {
		return v.normalized * length;
	}
	
	public static Vector2 WithScale(this Vector2 v, Vector2 scale) {
		return Vector2.Scale(v, scale);
	}
}
