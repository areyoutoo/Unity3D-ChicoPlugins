using UnityEngine;

/// <summary>
/// TweenCam that follows a target Transform.
/// </summary>
[RequireComponent(typeof(Camera))]
[AddComponentMenu("ChicoPlugins/TweenCam/ChaseCam")]
public class ChaseCam : TweenCam {
    /// <summary>
    /// Which Transform do we chase?
    /// </summary>
    /// <remarks>
    /// Can populate from inspector, or we'll try to pick on in OnStart.
    /// </remarks>
    public Transform target;
    
    private Transform lastTarget;
    private TweenState tween;
    
    /// <summary>
    /// While true, SetState state so that CamState pos/target vectors are in 
    /// target's local space.
    /// </summary>
    protected bool applyChaseOffset = true;
    
    protected override void OnStart() {
        //without target, find one!
        //   - parent (preferred)
        //   - self (fallback)
        if (target == null) {
            if (transform.parent != null) {
                target = transform.parent;
            } else {
                target = transform;
            }
        }
    }
    
    /// <summary>
    /// Modifies state so that pos/target vectors are in target's local space.
    /// </summary>
    protected override void SetState(CamState state) {
        Vector3 offset;
        if (tween.Tick(GetDeltaTime())) {
			Vector3 diff = lastTarget.position - target.position;
            offset = diff * (1f - tween.perc);
        } else {
            offset = Vector3.zero;
        }
        
        if (applyChaseOffset) {
            state.pos += offset; //FIXME: there's a bug somewhere in here about lerping between two targets
            state.target += offset;
        }
        base.SetState(state);
    }
    
    /// <summary>
    /// Tweens us toward another transform over time.
    /// </summary>
    /// <param name="newTarget">Transform to chase.</param>
    /// <param name="duration">Tween duration.</param>
    public void SwitchTarget(Transform newTarget, float duration=1f) {
		OnSwitchTarget();
        lastTarget = target;
		target = newTarget;
        tween.Reset(duration);
    }
    
    /// <summary>
    /// Snaps us to another transform immediately.
    /// </summary>
    /// <param name="newTarget">Transform to chase.</param>    
    public void SnapTarget(Transform newTarget) {
		OnSnapTarget();
        lastTarget = target;
        target = newTarget;
        tween.Stop();
    }
	
	protected virtual void OnSwitchTarget() {}
	protected virtual void OnSnapTarget() {}
	
	
}