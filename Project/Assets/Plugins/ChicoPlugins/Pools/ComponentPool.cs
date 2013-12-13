using UnityEngine;
using System.Collections.Generic;

public abstract class ComponentPool<T> : ComponentPoolBase where T : UnityEngine.Component {
	const int MAX_CLONE_COUNT = 128;
	int nextCloneCount = 2;
	
	public static int globalCopiesPending { get; protected set; }
	int copiesPending;
	
	List<T> members;
	T backupMember;
	
	public int count { 
		get { return members.Count; }
	}
	
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
	
	protected void AddClones() {
		Info(string.Format("exhausted, instantiating clones ({0})", nextCloneCount));
		
		copiesPending += nextCloneCount;
		globalCopiesPending += nextCloneCount;
		nextCloneCount = Mathf.Min(nextCloneCount * 2, MAX_CLONE_COUNT);
		
		AddClones(copyRate);
	}
	
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
	}
	
	protected void Warn(string msg) {
		msg = string.Format("{0} '{1}' {2}", GetType(), id, msg);
		Debug.LogWarning(msg, this);
	}
	
	protected void Info(string msg) {
		msg = string.Format("{0} '{1}' {2}", GetType(), id, msg);
		Debug.Log(msg, this);
	}
	
	protected virtual void OnAwake() {}
	protected virtual void OnUpdate() {}
	protected virtual void OnAdd(T item) {}
	protected virtual void OnGetNext(T item) {}
}
