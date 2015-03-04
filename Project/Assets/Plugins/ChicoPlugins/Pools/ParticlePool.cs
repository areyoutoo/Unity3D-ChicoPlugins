using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Pool for ParticleSystem components.
/// </summary>
/// <remarks>
/// Any ParticleSystem released will be added to an internal watchlist; the list
/// is periodically checked, and any finished emitter will be returned to the
/// pool.
///
/// If activateOnGet is true, the pool will call Play() on any ParticleSystem it
/// releases; otherwise, you'll need to do that yourself.
///
/// In the current build, this pool does not handle nested emitters.
/// </remarks>
[AddComponentMenu("ChicoPlugins/Pools/Particle System")]
public class ParticlePool : ComponentPool<ParticleSystem> {
	[SerializeField] float _reclaimRate = 0.5f;
    
    /// <summary>
    /// How often should we check for finished emitters? (inspector)
    /// </summary>
	public float reclaimRate {
		get { return _reclaimRate; }
	}
	
	float timeToReclaim;
	
	List<ParticleSystem> watchlist = new List<ParticleSystem>();
	
    /// <summary>
    /// Force the pool to check for and reclaim any finished emitters.
    /// </summary>    
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
