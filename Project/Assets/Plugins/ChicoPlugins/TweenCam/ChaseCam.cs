using UnityEngine;

public class ChaseCam : TweenCam {
    public Transform target;
    
    private Transform lastTarget;
    private TweenState tween;
    
    protected bool applyChaseOffset = true;
    
    protected override void OnStart() {
        //without target, find one!
        //   - parent (preferred)
        //   - self (fallback)
        if (target == null && transform.parent != null) {
            if (transform.parent != null) {
                target = transform.parent;
            } else {
                target = transform;
            }
        }
    }
    
    protected override void SetState(CamState state) {
        Vector3 offset;
        if (tween.Tick(GetDeltaTime())) {
            offset = Vector3.Lerp(lastTarget.position, target.position, tween.perc);
        } else {
            offset = target.position;
        }
        
        if (applyChaseOffset) {
            state.pos += offset;
            state.target += offset;
        }
        base.SetState(state);
    }
    
    public void SwitchTarget(Transform newTarget, float duration=1f) {
        SnapTarget(newTarget);
        tween.Reset(duration);
    }
    
    public void SnapTarget(Transform newTarget) {
        lastTarget = target;
        target = newTarget;
        tween.Stop();
    }
}