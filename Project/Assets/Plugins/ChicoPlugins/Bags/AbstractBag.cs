using UnityEngine;
using System.Collections;

/// <summary>
/// Abstract. Base class for other bag types.
/// </summary>
public abstract class AbstractBag<T> {
	/// <summary>
	/// Returns next bag item. Certain bag types may implement side-effects.
	/// </summary>
	public abstract T GetNext();
	
	/// <summary>
	/// Good-faith representation of bag's current contents.
	/// </summary>
	public abstract int count { get; }
}
