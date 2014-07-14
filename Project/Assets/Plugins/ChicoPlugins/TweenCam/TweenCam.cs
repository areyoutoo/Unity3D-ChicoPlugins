using UnityEngine;

/// <summary>
/// Tweening camera.
/// </summary>
/// <remarks>
/// TweenCam uses "CamState" structs to govern three parameters:
///
/// <list>
///   <item>
///     <description>Where is the camera?</description>
///   </item>
///   <item>
///     <description>What is the camera looking at?</description>
///   </item>
///   <item>
///     <description>What is the camera's FOV? (or size, if orthographic)</description>
///   </item>
/// </list>
///
/// <para>
/// This avoids directly worrying about complicated quaternion operations. All 
/// you need to know is where your camera is and what it's looking at.
/// </para>
///
/// <para>
/// You provide TweenCam with a delegate to build/modify CamState once per 
/// frame.
/// </para>
///
/// <para>
/// When changing delegates, you can "snap" (instant change) or "switch" 
/// (tweened change). While tweening, TweenCam will call both delegates and lerp
/// between the two results.
/// </para>
///
/// <para>
/// I recommend inheriting from TweenCam; here's a simple example:
/// </para>
///
/// <code>
/// public class ExampleCam : TweenCam {
///     public Transform player; //set in inspector
///     
///     protected override void OnUpdate() {
///         if (Input.GetKeyDown(KeyCode.Alpha1)) {
///             SwitchFunc(ShoulderCam, 2f);
///         } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
///             SwitchFunc(BirdsEyeCam, 1f);
///         }
///     }
/// 
///     //pos: behind shoulder
///     //target: slightly ahead
///     CamState ShoulderCam(CamState state) {
///         state.pos = player.position + player.back + Vector3.up;
///         state.target = player.position + player.forward;
///         return state;
///     }
///     
///     //pos: mostly overhead
///     //target: slightly in front
///     CamState BirdsEyeCam(CamState state) {
///         state.pos = player.position + player.back * 0.1f + Vector3.up * 2f;
///         state.target = player.position + player.forward * 0.2f;
///         return state;
///     }
/// }
/// </code>
///
/// </remarks>
[RequireComponent(typeof(Camera))]
[AddComponentMenu("ChicoPlugins/TweenCam/TweenCam")]
public class TweenCam : MonoBehaviour {  
    /// <summary>
    /// Timer for mode transitions.
    /// </summary>
    private TweenState tween;
    
    /// <summary>
    /// Camera's state for one frame.
    /// </summary>
    public struct CamState {
        /// <summary>
        /// Where is the camera?
        /// </summary>
        public Vector3 pos;
        
        /// <summary>
        /// What is the camera looking at?
        /// </summary>   
        public Vector3 target;
		
		/// <summary>
		/// Hint vector used for Transform.LookAt().
		/// </summary>
		public Vector3 up;
        
        /// <summary>
        /// Camera's FOV (or ortho size).
        /// </summary> 
        public float fov;
        
        public CamState(Vector3 pos, Vector3 target, float fov) : this(pos, target, fov, Vector3.up) {
        }
		
		public CamState(Vector3 pos, Vector3 target, float fov, Vector3 up) {
			this.pos = pos;
			this.target = target;
			this.fov = fov;
			this.up = up;
		}
    }
    
    /// <summary>
    /// Delegate to process desired CamState.
    /// </summary>        
    private System.Func<CamState, CamState> stateFunc;
    
    /// <summary>
    /// Previous stateFunc. Used in lerping.
    /// </summary>        
    private System.Func<CamState, CamState> lastStateFunc;
    
    /// <summary>
    /// CamState for previous frame.
    /// </summary>        
    private CamState lastState;
    
    /// <summary>
    /// Cached reference to Camera component.
    /// </summary>        
    private Camera cam;

    protected void Awake() {
        cam = GetComponent<Camera>();
        OnAwake();
    }
    
    protected void Start() {
        tween = new TweenState(0f);
        lastState = new CamState(transform.position, transform.position + transform.forward, GetFOV());
        lastStateFunc = stateFunc = GetStateDefault;
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
			float param = InterpParam(tween.perc);
            state = InterpState(a, b, param);                
        } else {
            state = stateFunc(state);
        }
        
        SetState(state);
        lastState = state;
    }
	
	/// <summary>
	/// Optional override to modify parameter to InterpState; otherwise the tween control is linear. Called once per frame during tweens.
	/// </summary>
	protected virtual float InterpParam(float param) {
		return param;
	}
	
	/// <summary>
	/// Optional override to control interpolation between two cam states; otherwise the tween is a lerp. Called once per frame during tweens.
	/// </summary>
	/// <returns>
	/// State to assign for this frame.
	/// </returns>
	/// <param name='a'>
	/// State before tweening.
	/// </param>
	/// <param name='b'>
	/// State after tweening.
	/// </param>
	/// <param name='t'>
	/// Tween param, generated by InterpParam.
	/// </param>
	protected virtual CamState InterpState(CamState a, CamState b, float t) {
		a.pos = Vector3.Lerp(a.pos, b.pos, t);
		a.target = Vector3.Lerp(a.target, b.target, t);
		a.fov = Mathf.Lerp(a.fov, b.fov, t);
		return a;
	}
    
    /// <summary>
    /// Apply CamState for current frame.
    /// </summary>
    /// <remarks>
    /// Useful to override in case you want last-minute edits, such as clamping.
    /// </remarks>
    /// <param name="state">Target state, post-tween.</param>
    protected virtual void SetState(CamState state) {
        transform.position = state.pos;
        transform.LookAt(state.target, state.up);
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
    
    /// <summary>
    /// Default CamState delegate, used before any switches are called.
    /// </summary>
    /// <param name="state">CamState from previous frame.</param>
    protected virtual CamState GetStateDefault(CamState state) {
        return state;
    }
    
    /// <summary>
    /// Called at the end of Awake().
    /// </summary>
    protected virtual void OnAwake() {}
    
    /// <summary>
    /// Called at the end of Start().
    /// </summary>
    protected virtual void OnStart() {}
    
    /// <summary>
    /// Called during Update(), just before we update desired CamState.
    /// </summary>        
	protected virtual void OnUpdate() {}
    
    /// <summary>
    /// Measure time with Time.deltaTime by default; override to change.
    /// </summary>
    protected virtual float GetDeltaTime() {
        return Time.deltaTime;
    }
    
    /// <summary>
    /// Tween to another CamState delegate over time.
    /// </summary>    
    /// <param name="inStateFunc">CamState delegate to switch to.</param>
    /// <param name="duration">Tween duration (in seconds).</param>
    protected void SwitchFunc(System.Func<CamState,CamState> inStateFunc, float duration=1f) {
        SnapFunc(inStateFunc);
        tween.Reset(duration);
    }
    
    /// <summary>
    /// Snap to another CamState delegate immediately.
    /// </summary> 
    /// <param name="inStateFunc">CamState delegate to switch to.</param>
    protected void SnapFunc(System.Func<CamState,CamState> inStateFunc) {
        tween.Stop();
        lastStateFunc = stateFunc;
        stateFunc = inStateFunc;
    }
}