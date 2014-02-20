using UnityEngine;
using System.Collections;

/// <summary>
/// Extension methods for Unity's Vector3 class.
/// </summary>
public static class Vector3Extensions {
    /// <summary>
    /// Copies this vector, overrides the copy's X.
    /// </summary>
	public static Vector3 WithX(this Vector3 v, float x) {
		return new Vector3(x, v.y, v.z);
	}
	
    /// <summary>
    /// Copies this vector, overrides the copy's Y.
    /// </summary>    
	public static Vector3 WithY(this Vector3 v, float y) {
		return new Vector3(v.x, y, v.z);
	}
	
    /// <summary>
    /// Copies this vector, overrides the copy's Z.
    /// </summary>    
	public static Vector3 WithZ(this Vector3 v, float z) {
		return new Vector3(v.x, v.y, z);
	}
	
    /// <summary>
    /// Copies this vector, multiplies the copy component-wise with another vector.
    /// </summary>
    /// <remarks>
    /// There is no float/int override because that's just scalar multiplication. ;)
    /// </remarks>
	public static Vector3 WithScale(this Vector3 v, Vector3 other) {
		return Vector3.Scale(v, other);
	}
	
    /// <summary>
    /// Copies this vector, overrides the copy's length.
    /// </summary>    
	public static Vector3 WithLength(this Vector3 v, float length) {
		return v.normalized * length;
	}
	
    /// <summary>
    /// Returns Vector2 with matching (x,y).
    /// </summary>    
	public static Vector2 ToVector2(this Vector3 v) {
		return new Vector2(v.x, v.y);
	}
}
