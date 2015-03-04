using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base class for generic object pools.
/// </summary>
/// <remarks>
/// Object pooling allows for significant performance gains: spawning objects
/// from prefabs can be very expensive, and this system allows you to recycle
/// those prefabs.
///
/// Pools are organized based on the scene hierarchy: a parent GameObject with
/// a ComponentPool attached will act as a "folder" containing its pool members.
/// At the start of gameplay, the ComponentPool will search its immediate
/// children for any which have the desired component type. Later, you can ask
/// the pool for one of those members, add members, remove members, and so on.
///
/// These pools are designed with performance in mind: once the pool is empty
/// (or "exhausted"), you have the option of automatically creating X new clones
/// per frame, manually instructing it to create new clones, or finally just
/// returning null for any further requests.
/// </remarks>
public abstract class ComponentPool<T> : ComponentPoolBase where T : UnityEngine.Component {
	const int MAX_CLONE_COUNT = 128;
	int nextCloneCount = 2;
	
	public static int globalCopiesPending { get; protected set; }
	public int copiesPending { get; protected set; }
	
	List<T> members;
	T backupMember;
	
	public int count { 
		get { return members.Count; }
	}
	
    /// <summary>
    /// Request one pool member.
    /// </summary>
    /// <remarks>
    /// Behavior once the pool is empty depends on the value of copyOnEmpty.
    /// </remarks>
	public T GetNext() {
		T item;
		if (GetNextInternal(out item)) {
			if (activateOnGet) {
				item.gameObject.SetActive(true);
			}
			OnGetNext(item);
		}
		return item;
	}
	
    /// <summary>
    /// Request one pool member at the given position.
    /// </summary>
    /// <remarks>
    /// Behavior once the pool is empty depends on the value of copyOnEmpty.
    /// </remarks>    
	public T GetNextAt(Vector3 position) {
		T item;
		if (GetNextInternal(out item)) {
			item.transform.position = position;
			if (activateOnGet) {
				item.gameObject.SetActive(true);
			}
			OnGetNext(item);
		}
		return item;
	}
	
    /// <summary>
    /// Request one pool member at the given position and rotation.
    /// </summary>
    /// <remarks>
    /// Behavior once the pool is empty depends on the value of copyOnEmpty.
    /// </remarks>    
	public T GetNextAt(Vector3 position, Quaternion rotation) {
		T item;
		if (GetNextInternal(out item)) {
			item.transform.position = position;
			item.transform.rotation = rotation;
			if (activateOnGet) {
				item.gameObject.SetActive(true);
			}
			OnGetNext(item);
		}
		return item;
	}
	
    /// <summary>
    /// Add one member to the pool.
    /// </summary>
    /// <remarks>
    /// If copyOnEmpty or copyOnStart are true, the first member added will be
    /// kept "on ice" for use with Instantiate().
    /// </remarks>    
	public void Add(T item) {
		if (item == null) {
			Warn("ignoring add: invalid item");
		} else {
			if ((copyOnEmpty || copyOnStart > 0) && backupMember == null) {
				backupMember = item;
			} else {
				members.Add(item);
				OnAdd(item);
			}
			item.transform.parent = transform;
			item.gameObject.SetActive(false);
		}
	}
	
    /// <summary>
    /// Immediately clones more members.
    /// </summary>
    /// <remarks>
    /// If the pool has no backup member to use for cloning, the call will fail with a warning.
    /// </remarks>
	public void AddClones(int count) {
		if (backupMember == null) {
			Warn("tried to add clones, but has no base item to clone");
		} else {
			while (count-- > 0) {
				T item = (T)Instantiate(backupMember, backupMember.transform.position, backupMember.transform.rotation);
				item.name = backupMember.name;
				Add(item);
				
				if (copiesPending > 0) {
					copiesPending -= 1;
					globalCopiesPending -= 1;
				}
			}
		}
	}
	
    /// <summary>
    /// Immediately clone copyRate members; queue up more clones to be done on later frames.
    /// </summary>
    /// <remarks>
    /// The first call to AddClones will queue 2 clones; additional calls will
    /// double the number, up to 128.
    /// </remarks>
	protected void AddClones() {
		Info(string.Format("exhausted, instantiating clones ({0})", nextCloneCount));
		
		copiesPending += nextCloneCount;
		globalCopiesPending += nextCloneCount;
		nextCloneCount = Mathf.Min(nextCloneCount * 2, MAX_CLONE_COUNT);
		
		AddClones(copyRate);
	}
	
    /// <summary>
    /// Called by the GetNext() functions. Manages list add/remove, cloning, etc.
    /// </summary>    
	protected bool GetNextInternal(out T item) {
		bool success = false;
		if (count > 0) {
			int i = Random.Range(0, count);
			item = members[i];
			members.RemoveAt(i);
			success = true;
		} else if (copyOnEmpty) {
			AddClones();
			if (count > 0) {
				success = GetNextInternal(out item);
			} else {
				Warn("has copyOnEmpty, but copy failed");
				item = default(T);
			}
		} else {
			Warn("is empty (add more items or enable copyOnEmpty)");
			item = default(T);
			success = false;
		}
		
		return success;
	}
	
	protected void Awake() {
		members = new List<T>();
		foreach (Transform child in transform) {
			T item = child.GetComponent<T>();
			if (item != null) {
				Add(item);
			}
		}
		if (copyOnStart > 0) {
			copiesPending = copyOnStart;
			AddClones(copyRate);
		}
		PoolManager.Add(this);
		OnAwake();
	}
	
	protected void Update() {
		if (copiesPending > 0) {
			AddClones(copyRate);
		}
		OnUpdate();
	}
	
	protected void OnDestroy() {
		PoolManager.Remove(id);
		globalCopiesPending -= copiesPending;
	}
	
    /// <summary>
    /// Formatted alias for Debug.LogWarning().
    /// </summary>
	protected void Warn(string msg) {
		msg = string.Format("{0} '{1}' {2}", GetType(), id, msg);
		Debug.LogWarning(msg, this);
	}
	
    /// <summary>
    /// Formatted alias for Debug.Log().
    /// </summary>    
	protected void Info(string msg) {
		msg = string.Format("{0} '{1}' {2}", GetType(), id, msg);
		Debug.Log(msg, this);
	}
	
    /// <summary>
    /// Optional override. Called at the end of Awake().
    /// </summary>    
	protected virtual void OnAwake() {}
    
    /// <summary>
    /// Optional override. Called at the end of Update().
    /// </summary>    
	protected virtual void OnUpdate() {}
    
    /// <summary>
    /// Optional override. Called near the end of Add(), just before new member is disabled.
    /// </summary>
    /// <remarks>
    /// Note this will NOT be called for the backup "on ice" member.
    /// </remarks>
	protected virtual void OnAdd(T item) {}
    
    /// <summary>
    /// Optional override. Called by GetNext() and its siblings JUST before returning the member.
    /// </summary>
    /// <remarks>
    /// Useful if you need to do something with the object before returning it.
    ///
    /// Note this will NOT be called if the GetNext call is returning null.
    /// </remarks>
	protected virtual void OnGetNext(T item) {}

	/// <summary>
	/// Static equivalent of GetNext.
	/// </summary>
	/// <returns>
	/// True if you got an item; false otherwise (no such pool, pool is empty, etc.)
	/// </returns>
    public static bool TryGetNext(string poolName, out T item) {
        ComponentPool<T> pool = PoolManager.Get(poolName) as ComponentPool<T>;
        if (pool != null) {
            item = pool.GetNext();
        } else {
            item = null;
        }
        return item != null;
    }
    
	/// <summary>
	/// Static equivalent of GetNextAt.
	/// </summary>
	/// <returns>
	/// True if you got an item; false otherwise (no such pool, pool is empty, etc.)
	/// </returns>    
    public static bool TryGetNextAt(string poolName, Vector3 pos, out T item) {
        ComponentPool<T> pool = PoolManager.Get(poolName) as ComponentPool<T>;
        if (pool != null) {
            item = pool.GetNextAt(pos);
        } else {
            item = null;
        }
        return item != null;    
    }

	/// <summary>
	/// Static equivalent of GetNext.
	/// </summary>
	/// <returns>
	/// True if the add succeeded; false otherwise (no such pool, pool is wrong type, etc.)
	/// </returns>
    public static bool TryAdd(string poolName, T item) {
    	ComponentPool<T> pool = PoolManager.Get(poolName) as ComponentPool<T>;
    	if (pool != null) {
    		pool.Add(item);
    	}
    	return pool != null;
    }	
}
