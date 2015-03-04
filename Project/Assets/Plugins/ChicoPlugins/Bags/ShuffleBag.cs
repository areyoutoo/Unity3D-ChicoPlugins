using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Bag returns items at random, like a deck of cards.
/// </summary>
/// <remarks>
/// Note that the big will automatically refill itself once emptied.
/// </remarks>
public class ShuffleBag<T> : RandomBag<T> {
	/// <summary>
	/// An internal list, used by Refill().
	/// <summary> 	
	protected List<T> backups;
	
	/// <summary>
	/// Alternative to count.
	/// </summary>
	public int backupCount {
		get {
			return backups.Count;
		}
	}
	
	/// <summary>
	/// Returns (and removes) a random item. Refills from backup list if empty.
	/// </summary>	
	public override T GetNext()
	{
		if (members.Count < 1 && backups.Count > 0) {
			Refill();
		}
		
		if (members.Count > 0) {
			int i = Random.Range(0, members.Count);
			T item = members[i];
			members.RemoveAt(i);
			return item;
		} else {
			return base.GetNext ();
		}
	}

	/// <summary>
	/// Items added to the bag are also added to an internal backup list.
	/// </summary>
	public override void Add(T item) {
		base.Add(item);
		backups.Add(item);
	}
	
	/// <summary>
	/// Removes item (including internal backup).
	/// </summary>	
	public override bool Remove(T item) {
		bool m = base.Remove(item);
		bool b = backups.Remove(item);
		return m || b;
	}
	
	/// <summary>
	/// Refills the bag.
	/// </summary>	
	/// <remarks>
	/// After refill, bag will include all items that have ever been added, unless they are explicitly removed beforehand.
	/// </remarks>
	public void Refill() {
		members.Clear();
		members.AddRange(backups);
	}
	
	public ShuffleBag() : base() {
		backups = new List<T>();
	}
	
	/// <summary>Memory optimization: request initial capacity.<summary>
	public ShuffleBag(int capacity) : base(capacity) {
		backups = new List<T>(capacity);
	}
	
	/// <summary>
	/// Add all items to the new bag.
	/// <summary> 	
	public ShuffleBag(IEnumerable<T> items) : this() {
		AddRange(items);
	}
	
	/// <summary>
	/// Add all items to the new bag.
	/// <summary> 		
	public ShuffleBag(params T[] items) : this() {
		AddRange(items);
	}
}
