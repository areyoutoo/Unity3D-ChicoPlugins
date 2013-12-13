using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Bag returns item in random order, refills itself once empty.
/// </summary>
public class ShuffleBag<T> : RandomBag<T> {
	protected List<T> backups;
	
	public int backupCount {
		get {
			return backups.Count;
		}
	}
	
	public override T GetNext()
	{
		if (members.Count < 1) {
			Refill();
		}
		return base.GetNext ();
	}
	
	public override void Add(T item) {
		base.Add(item);
		backups.Add(item);
	}
	
	public override bool Remove(T item) {
		bool m = base.Remove(item);
		bool b = backups.Remove(item);
		return m || b;
	}
	
	public void Refill() {
		members.Clear();
		members.AddRange(backups);
	}
	
	public ShuffleBag() : base() {
		backups = new List<T>();
	}
	
	public ShuffleBag(int capacity) : base(capacity) {
		backups = new List<T>(capacity);
	}
	
	public ShuffleBag(IEnumerable<T> items) : this() {
		AddRange(items);
	}
	
	public ShuffleBag(params T[] items) : this() {
		AddRange(items);
	}
}
