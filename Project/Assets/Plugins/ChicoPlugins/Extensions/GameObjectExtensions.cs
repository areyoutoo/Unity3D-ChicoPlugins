using UnityEngine;
using System.Collections;

/// <summary>
/// Extension methods for Unity's GameObject class.
/// </summary>
public static class GameObjectExtensions {
	/// <summary>
	/// Sets the layer recursively by index.
	/// </summary>
	public static void SetLayerRecursively(this GameObject root, int layer) {
		root.layer = layer;
		foreach (Transform child in root.transform) {
			child.gameObject.SetLayerRecursively(layer);
		}
	}
	
	/// <summary>
	/// Sets layer recursively by name.
	/// </summary>
	public static void SetLayerRecursively(this GameObject root, string layerName) {
		int layer = LayerMask.NameToLayer(layerName);
		root.SetLayerRecursively(layer);
	}
	
	/// <summary>
	/// Returns attached component of type T, creating one if it does not exist.
	/// </summary>
	/// <remarks>
	/// If a component is attached, but has been destroyed, a new one will be added.
	/// </remarks>
	public static T GetOrAddComponent<T>(this GameObject root) where T : Component {
		T component = root.GetComponent<T>();
		if (component == null) { //overloaded equals: Unity returns true when comparing null to destroyed objects
			component = root.AddComponent<T>();
		}
		return component;
	}
	
	/// <summary>
	/// Destroys first attached component of type T.
	/// </summary>
	/// <param name="immediate">
	/// Calls DestroyImmediate() instead of Destroy(); useful in editor.
	/// </param>
	/// <returns>
	/// True if any components were destroyed, false otherwise.
	/// </returns>
	/// <remarks>
	/// Safe to call if no such component is attached.
	/// </remarks>
	public static bool DestroyComponent<T>(this GameObject root, bool immediate=false) where T : Component {
		T component = root.GetComponent<T>();
		
		bool found = component != null;
		if (found) {
			if (immediate) {
				GameObject.DestroyImmediate(component, false);
			} else {
				GameObject.Destroy(component);
			}
		}
		
		return found;
	}
	
	/// <summary>
	/// Destroys all attached components of type T.
	/// </summary>
	/// <param name="immediate">
	/// Calls DestroyImmediate() instead of Destroy(); useful in editor.
	/// </param>
	/// <returns>
	/// True if any components were destroyed, false otherwise.
	/// </returns>
	/// <remarks>
	/// Safe to call if no such component is attached.
	/// </remarks>
	public static bool DestroyComponents<T>(this GameObject root, bool immediate=false) where T : Component {
		T[] components = root.GetComponents<T>();
		
		bool found = components.Length > 0;
		if (found) {
			for (int i=0; i<components.Length; i++) {
				if (immediate) {
					GameObject.DestroyImmediate(components[i], false);
				} else {
					GameObject.Destroy(components[i]);
				}
			}
		}
		
		return found;
	}
	
	/// <summary>
	/// Destroys all attached components of type T, recursively.
	/// </summary>
	/// <param name="immediate">
	/// Calls DestroyImmediate() instead of Destroy(); useful in editor.
	/// </param>
	/// <returns>
	/// True if any components were destroyed, false otherwise.
	/// </returns>
	/// <remarks>
	/// Safe to call if no such component is attached.
	/// </remarks>
	public static bool DestroyComponentsInChildren<T>(this GameObject root, bool immediate=false) where T : Component {
		T[] components = root.GetComponentsInChildren<T>();
		
		bool found = components.Length > 0;
		if (found) {
			for (int i=0; i<components.Length; i++) {
				if (immediate) {
					GameObject.DestroyImmediate(components[i], false);
				} else {
					GameObject.Destroy(components[i]);
				}
			}
		}
		
		return found;
	}
	
	/// <summary>
	/// Instantiate a copy, make the copy a child of this GameObject.
	/// </summary>
	/// <returns>
	/// The child.
	/// </returns>
	public static GameObject InstantiateChild(this GameObject root, GameObject prefab) {
		return root.InstantiateChild(prefab, Vector3.zero, Quaternion.identity);
	}
	
	/// <summary>
	/// Instantiate a copy, make the copy a child of this GameObject.
	/// </summary>
	/// <returns>
	/// The child.
	/// </returns>
	/// <param name='position'>
	/// Position offset.
	/// </param>
	/// <param name='rotation'>
	/// Rotation offset.
	/// </param>
	/// <param name='space'>
	/// If Space.Self or omitted, position and rotation are added to the parent's; if Space.World, both are absolute.
	/// </param>
	public static GameObject InstantiateChild(this GameObject root, GameObject prefab, Vector3 position, Quaternion rotation, Space space=Space.Self) {
		GameObject child;
		if (space == Space.World) {
			child = (GameObject)GameObject.Instantiate(prefab, position, rotation);
			child.transform.parent = root.transform;
		} else {
			child = (GameObject)GameObject.Instantiate(prefab, root.transform.position, root.transform.rotation);
			child.transform.parent = root.transform;
			child.transform.localPosition = position;
			child.transform.localRotation = rotation;
		}
		
		return child;
	}

	/// <summary>
	/// Get the outermost bounds of all renderers (including children).
	/// </summary>
	public static Bounds GetRendererBounds(this GameObject root) {
		return Boundsx.EncapsulateAll(root.GetComponentsInChildren<Renderer>());
	}
	
	/// <summary>
	/// Gets the outermost bounds of all colliders (including children).
	/// </summary>
	public static Bounds GetColliderBounds(this GameObject root) {
		return Boundsx.EncapsulateAll(root.GetComponentsInChildren<Collider>());
	}
	
	/// <summary>
	/// Call some delegate for each object in GetComponents<T>.
	/// </summary>
	public static void ForAllComponents<T>(this GameObject root, System.Action<T> action) where T : Component {
		foreach (T component in root.GetComponents<T>()) {		
			action(component);
		}
	}
	
	/// <summary>
	/// Call some delegate for each object in GetComponentsInChildren<T>.
	/// </summary>
	public static void ForAllComponentsInChildren<T>(this GameObject root, System.Action<T> action) where T : Component {
		foreach (T component in root.GetComponentsInChildren<T>()) {
			action(component);
		}
	}
}
