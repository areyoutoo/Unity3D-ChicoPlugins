using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Abstract. AbstractBag with a simple Add method.
/// </summary>
public abstract class SimpleBag<T> : AbstractBag<T> {

	/// <summary>
	/// Adds one item to the bag. Analogous to List<T>.Add()
	/// <summary> 	
	public abstract void Add(T item);
	
	/// <summary>
	/// Removes one item from the bag. Analogous to List<T>.Remove()
	/// <summary> 		
	public abstract bool Remove(T item);
	
	/// <summary>
	/// Calls Remove() on set.
	/// <summary> 		
	public void RemoveRange(IEnumerable<T> items) {
		foreach (var item in items) {
			Remove(item);
		}
	}
	/// <summary>
	/// Calls Add() on set.
	/// <summary> 	
	public void AddRange(IEnumerable<T> items) {
		foreach (var item in items) {
			Add(item);
		}
	}
}
