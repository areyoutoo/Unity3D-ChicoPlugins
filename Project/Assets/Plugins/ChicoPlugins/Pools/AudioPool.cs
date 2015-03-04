using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// AudioSource pool.
/// </summary>
/// <remarks>
/// Any AudioSource released will be added to an internal watchlist; the list
/// is periodically checked, and any finished sound will be returned to the
/// pool.
///
/// If activateOnGet is true, the pool will call Play() on any AudioSource it
/// releases; otherwise, you'll need to do that yourself.
/// </remarks>
[AddComponentMenu("ChicoPlugins/Pools/Audio Source")]
public class AudioPool : ComponentPool<AudioSource> {
	[SerializeField] float _reclaimRate = 0.5f;
	public float reclaimRate {
		get { return _reclaimRate; }
	}
	
	float timeToReclaim;
	
	List<AudioSource> watchlist = new List<AudioSource>();
	
	public void Reclaim() {
		for (int i=watchlist.Count-1; i >= 0; i--) {
			AudioSource item = watchlist[i];
			if (!item.isPlaying) {
				watchlist.RemoveAt(i);
				Add(item);
			}
		}
	}
	
	protected override void OnGetNext(AudioSource item) {
		watchlist.Add(item);
		if (activateOnGet) {
			item.Play();
		}
		enabled = true;
	}
	
	protected override void OnAwake() {
		timeToReclaim = reclaimRate;
	}
	
	protected override void OnUpdate() {
		timeToReclaim -= Time.time;
		if (timeToReclaim < 0f) {
			timeToReclaim = _reclaimRate;
			Reclaim();
		}
	}
}
