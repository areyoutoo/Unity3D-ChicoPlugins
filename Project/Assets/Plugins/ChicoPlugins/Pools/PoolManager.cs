using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Easy access to any ComponentPool in your scene.
/// </summary>
public static class PoolManager {
	static Dictionary<string, ComponentPoolBase> poolMap = new Dictionary<string, ComponentPoolBase>();
	
    /// <summary>
    /// Notify manager that new pool exists.
    /// </summary>    
    /// <remarks>
    /// Should be called by each pool once it has initialized.
    ///
    /// If a pool with the same name is already registered, the newer pool will be ignored.
    /// </remarks>
	public static void Add(ComponentPoolBase pool) {
		string id = pool.id;
		if (poolMap.ContainsKey(id)) {
			string msg = string.Format("Ignoring duplicate pool '{0}'", id);
			Debug.LogWarning(msg);
		} else {
			poolMap.Add(id, pool);
		}
	}
	
    /// <summary>
    /// Notify manager that pool has been destroyed.
    /// </summary>
    /// <remarks>
    /// Should be called by each pool as it is destroyed.
    /// </remarks>
	public static void Remove(string id) {
		poolMap.Remove(id);
	}
	
    /// <summary>
    /// Provides global access point for all pools in the scene.
    /// </summary>
    /// <remarks>
    /// For access to a specific pool type's functions, use Get<T> instead.
    /// </remarks>
	public static ComponentPoolBase Get(string id) {
		ComponentPoolBase pool;
		if (!poolMap.TryGetValue(id, out pool)) {
			string msg = string.Format("No such pool '{0}'", id);
			Debug.LogWarning(msg);
		}
		return pool;
	}
	
    /// <summary>
    /// Main access point. Provides global access point for all pools in the scene.
    /// </summary>
	public static T Get<T>(string id) where T : ComponentPoolBase {
		T pool = null;
		ComponentPoolBase bPool;
		if (!poolMap.TryGetValue(id, out bPool)) {
			string msg = string.Format("No such pool '{0}'", id);
			Debug.LogWarning(msg);
		} else {
			pool = bPool as T;
			if (pool == null) {
				string msg = string.Format("Pool '{0}' is not of type '{1}'", id, typeof(T));
				Debug.LogWarning(msg, bPool);
			}
		}
		return pool;
	}
}