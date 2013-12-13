using UnityEngine;
using System.Collections.Generic;


public class ParticlePool : ComponentPool<ParticleSystem> {
	[SerializeField] float _reclaimRate = 0.5f;
	public float reclaimRate {
		get { return _reclaimRate; }
	}
	
	float timeToReclaim;
	
	List<ParticleSystem> watchlist = new List<ParticleSystem>();
	
	public void Reclaim() {
		for (int i=watchlist.Count-1; i >= 0; i--) {
			ParticleSystem item = watchlist[i];
			if (!item.isPlaying) {
				watchlist.RemoveAt(i);
				Add(item);
			}
		}
	}
	
	protected override void OnGetNext(ParticleSystem item) {
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
