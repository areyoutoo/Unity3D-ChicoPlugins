using UnityEngine;
using System.Collections;

/// <summary>
/// Extension methods for Unity's Vector2 class.
/// </summary>
public static class Vector2Extensions {
    /// <summary>
    /// Creates a Vector3 with matching (x,y), requested (z).
    /// </summary>
	public static Vector3 ToVector3(this Vector2 v, float z) {
		return new Vector3(v.x, v.y, z);
	}
	
    /// <summary>
    /// Copies this vector, overrides the copy's X.
    /// </summary>    
	public static Vector3 WithX(this Vector2 v, float x) {
		return new Vector2(x, v.y);
	}
	
    /// <summary>
    /// Copies this vector, overrides the copy's Y.
    /// </summary>    
	public static Vector3 WithY(this Vector2 v, float y) {
		return new Vector2(v.x, y);
	}
	
    /// <summary>
    /// Copies this vector, overrides the copy's length.
    /// </summary>    
	public static Vector2 WithLength(this Vector2 v, float length) {
		return v.normalized * length;
	}
	
    /// <summary>
    /// Copies this vector, multiplies the copy component-wise with another vector.
    /// </summary>
    /// <remarks>
    /// There is no float/int override because that's just scalar multiplication. ;)
    /// </remarks>
	public static Vector2 WithScale(this Vector2 v, Vector2 scale) {
		return Vector2.Scale(v, scale);
	}
}
