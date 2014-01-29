using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TweenCam : MonoBehaviour {  
    private TweenState tween;
    
    public struct CamState {
        public Vector3 pos;
        public Vector3 target;
        public float fov;
        
        public CamState(Vector3 pos, Vector3 target, float fov) {
            this.pos = pos;
            this.target = target;
            this.fov = fov;
        }
    }
    
    public System.Func<CamState, CamState> stateFunc;
    public System.Func<CamState, CamState> lastStateFunc;
    
    private CamState lastState;
    private Camera cam;
    
    protected void Awake() {
        cam = GetComponent<Camera>();
        OnAwake();
    }
    
    protected void Start() {
        tween = new TweenState(0f);
        lastState = new CamState(transform.position, transform.position + transform.forward, GetFOV());
        stateFunc = GetStateDefault;
        lastStateFunc = stateFunc;
        OnStart();
    }
    
    protected void Update() {
        CamState state = lastState;
		
		OnUpdate();

        if (tween.Tick(GetDeltaTime())) {
            CamState a = state;
            CamState b = state;
            a = lastStateFunc(a);
            b = stateFunc(b);
            state = LerpState(a, b, tween.perc);                
        } else {
            state = stateFunc(state);
        }
        
        SetState(state);
        lastState = state;
    }
	
	protected CamState LerpState(CamState a, CamState b, float t) {
		a.pos = Vector3.Lerp(a.pos, b.pos, t);
		a.target = Vector3.Lerp(a.target, b.target, t);
		a.fov = Mathf.Lerp(a.fov, b.fov, t);
		return a;
	}
    
    protected virtual void SetState(CamState state) {
        transform.position = state.pos;
        transform.LookAt(state.target);
        SetFOV(state.fov);
    }
    
    private float GetFOV() {
        return cam.orthographic ? cam.orthographicSize : cam.fieldOfView;
    }
    
    private void SetFOV(float input) {
        if (cam.orthographic) {
            cam.orthographicSize = input;
        } else {
            cam.fieldOfView = input;
        }
    }
    
    protected virtual CamState GetStateDefault(CamState state) {
        return state;
    }
    
    protected virtual void OnAwake() {}
    protected virtual void OnStart() {}
	protected virtual void OnUpdate() {}
    
    protected virtual float GetDeltaTime() {
        return Time.deltaTime;
    }
    
    protected void SwitchFunc(System.Func<CamState,CamState> inStateFunc, float duration=1f) {
        SnapFunc(inStateFunc);
        tween.Reset(duration);
    }
    
    protected void SnapFunc(System.Func<CamState,CamState> inStateFunc) {
        tween.Stop();
        lastStateFunc = stateFunc;
        stateFunc = inStateFunc;
    }
}