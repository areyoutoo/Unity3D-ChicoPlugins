using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for other bag types.
/// </summary>
public abstract class AbstractBag<T> {
	public abstract T GetNext();
	public abstract int count { get; }
}
