using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// AbstractBag with a simple Add method.
/// </summary>
public abstract class SimpleBag<T> : AbstractBag<T> {
	public abstract void Add(T item);
	public abstract bool Remove(T item);
	
	public void RemoveRange(IEnumerable<T> items) {
		foreach (var item in items) {
			Remove(item);
		}
	}

	public void AddRange(IEnumerable<T> items) {
		foreach (var item in items) {
			Add(item);
		}
	}
}
