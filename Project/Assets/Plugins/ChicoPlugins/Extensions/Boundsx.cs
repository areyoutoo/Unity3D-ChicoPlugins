using UnityEngine;
using System.Collections;

/// <summary>
/// Helper functions for Unity's Bounds class.
/// </summary>
public static class Boundsx {
	
	/// <summary>
	/// Returns a new bounds which encapsulates the set of input bounds.
	/// </summary>
	public static Bounds EncapsulateAll(Bounds[] bounds) {
		Bounds b;
		if (bounds.Length > 0) {
			b = new Bounds(bounds[0].center, bounds[0].size);
			for (int i=1; i<bounds.Length; i++) {
				b.Encapsulate(bounds[i]);
			}
		} else {
			b = new Bounds(Vector3.zero, Vector3.zero);
		}
		
		return b;
	}
	
	/// <summary>
	/// Returns a new bounds which encapsulates the set of renderers.
	/// </summary>
	public static Bounds EncapsulateAll(Renderer[] renderers) {
		return EncapsulateAll<Renderer>(renderers, r => r.bounds);
	}
	
	/// <summary>
	/// Returns a new bounds which encapsulates the set of colliders.
	/// </summary>
	public static Bounds EncapsulateAll(Collider[] colliders) {
		return EncapsulateAll<Collider>(colliders, c => c.bounds);
	}
	
	/// <summary>
	/// Helper function to encapsulate arbitrary bounds providers.
	/// </summary>
	static Bounds EncapsulateAll<T>(T[] args, System.Func<T,Bounds> boundsFunc) {
		Bounds b;
		if (args.Length > 0) {
			Bounds first = boundsFunc(args[0]);
			b = new Bounds(first.center, first.size);
			for (int i=1; i<args.Length; i++) {
				b.Encapsulate(boundsFunc(args[i]));
			}
		} else {
			b = new Bounds(Vector3.zero, Vector3.zero);
		}
		
		return b;
	}
}
