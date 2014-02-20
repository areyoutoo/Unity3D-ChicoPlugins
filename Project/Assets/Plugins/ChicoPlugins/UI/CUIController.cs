using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attach to camera, drives UI click behavior.
/// </summary>
/// <remarks>
///
/// <para>
/// Attempts to handle both touch and mouse input. Takes screen position from
/// mouse (or touches[0]), uses attached camera to raycast for any colliders on
/// same layer as the controller.
/// </para>
///
/// <para>
/// "Click" is perhaps a bit of a misnomer: this script interprets the cursor as
/// clicking if left-click is held (standalone) or there is at least one touch
/// (mobile).
/// </para>
///
/// <para>
/// One word of caution: on PC, the notion of "click" and "position" are 
/// distinct; the user can move the mouse without clicking. On mobile, touching 
/// *is* clicking, which may be confusing at first.
/// </para>
///
/// <para>
/// Typical setup: place a second "UI" camera in the level, set it up to render 
/// over the game scene. Attach CUIController to that camera. Recommend putting
/// all UI on a special layer, collecting all UI widgets under the camera.
/// </para>
///
/// <para>
/// Recommended camera settings:
///
/// <list>
///   <item>
///     <description>Orthographic view makes it easy to layer the UI.</description>
///   </item>
///   <item>
///     <description>Clear flags: background only.</description>
///   </item>
///   <item>
///     <description>Culling mask: render UI layer only.</description>
///   </item>
///   <item>
///     <description>Depth: render over the main scene camera.</description>
///   </item>
/// </list>
/// </para>
///
/// <para>
/// Set of hooks, called at most once per frame in the order listed:
///
/// <list>
///   <item>
///     <description>MouseDown: called once when user begins click</description>
///   </item>
///   <item>
///     <description>MouseUp: called once when user ends click</description>
///   </item>
///   <item>
///     <description>MouseRelease: called once when MouseDown and MouseUp targeted the same GameObject</description>
///   </item>
///   <item>
///     <description>MouseOut: called each time the mouse exits a target.</description>
///   </item>
///   <item>
///     <description>MouseIn: called each time the mouse changes targets.</description>
///   </item>
///   <item>
///     <description>MouseHold: called once per frame while click is held over and object.</description>
///   </item>
/// </list>
/// </para>
///
/// </remarks>
[RequireComponent(typeof(Camera))]
public class CUIController : MonoBehaviour {
    /// <summary>
    /// Cached camera reference (read-only).
    /// </summary>
    public Camera uiCamera { get; private set; }
    
    /// <summary>
    /// LayerMask used for raycast (read-only).
    /// </summary>    
    public int uiLayerMask { get; private set; }
    
    /// <summary>
    /// This frame, mouse cursor is over this GameObject (read-only).
    /// </summary>    
    public GameObject mouseTarget { get; private set; }
    
    /// <summary>
    /// Last frame, mouse cursor was over this GameObject (read-only).
    /// </summary>    
    public GameObject lastMouseTarget { get; private set; }
    
    /// <summary>
    /// This frame, mouse's screen position (read-only).
    /// </summary>    
    public Vector3 mousePos { get; private set; }
    
    /// <summary>
    /// Last frame, mouse's screen position (read-only).
    /// </summary>    
    public Vector3 lastMousePos { get; private set; }
    
    /// <summary>
    /// This frame, ray used for casting (read-only).
    /// </summary>    
    public Ray mouseRay { get; private set; }
    
    /// <summary>
    /// Last frame, ray used for casting (read-only).
    /// </summary>    
    public Ray lastMouseRay { get; private set; }
    
    /// <summary>
    /// This frame, is mouse pressed? (read-only)
    /// </summary>    
    public bool mouseDown { get; private set; }
    
    /// <summary>
    /// Last frame, was mouse pressed? (read-only)
    /// </summary>
    public bool lastMouseDown { get; private set; }
    
    /// <summary>
    /// mouseTarget during the frame we first clicked (read-only).
    /// </summary> 
    public GameObject mouseDownTarget { get; private set; }
    
    /// <summary>
    /// Is this the "main" CUIController in the scene? (inspector)
    /// </summary>    
    public bool isMain = false;
	
    /// <summary>
    /// Returns first "main" CUIController in the scene.
    /// </summary>    
	public static CUIController main { get; private set; }
    
    protected void Awake() {
        uiCamera = GetComponent<Camera>();
        uiLayerMask = 1 << gameObject.layer;
        
        if (isMain) {
            if (main != null) {
                Debug.LogWarning("Ignoring extra 'main' CUIController.", this);
            } else {
                main = this;
            }
        }
        
        OnAwake();
    }
    
    protected void Start() {
        if (main == null) {
            main = this;
        }
        OnStart();
    }
    
    protected void Update() {
        UpdateFrameValues();        
        OnUpdate();
        CacheLastFrameValues();
    }
    
    /// <summary>
    /// Override for custom click detection.
    /// </summary>
    /// <returns>True if clicking during this frame.</returns>
    protected virtual bool GetMouseDown() {
        #if UNITY_STANDALONE || UNITY_EDITOR
        return Input.GetMouseButton(0);
        #else
        return Input.touchCount > 0;
        #endif
    }
    
    /// <summary>
    /// Override for custom mouse position detection.
    /// </summary>
    /// <returns>Screen position for GetMouseRay (be careful to assign a z-component).</returns>
    protected virtual Vector3 GetMousePosition() {
        Vector3 mp = Input.mousePosition.WithZ(0.1f);
        
        #if !UNITY_STANDALONE
            #if UNITY_EDITOR
                if (!Input.GetMouseButton(0)) { mp = Vector3.zero; }
            #else
                if (Input.touchCount < 1) { mp = Vector3.zero; }
            #endif
        #endif
        
        return mp;
    }
    
    /// <summary>
    /// Override for custom raycast calculation.
    /// </summary>
    /// <returns>Ray to use for GetMouseTarget.</returns>
    protected virtual Ray GetMouseRay() {
        return uiCamera.ScreenPointToRay(mousePos);
    }
    
    /// <summary>
    /// Override for custom target picking.
    /// </summary>
    /// <returns>This frame's mouseTarget.</returns>
    protected virtual GameObject GetMouseTarget() {
        GameObject target = null;
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, 100000f, uiLayerMask)) {
            target = hit.collider.gameObject;
        }
        return target;
    }
    
    /// <summary>
    /// Called at the beginning of each frame, updates internal values.
    /// </summary>
    protected virtual void UpdateFrameValues() {
        mouseDown = GetMouseDown();
        mousePos = GetMousePosition();
        mouseRay = GetMouseRay();
        mouseTarget = GetMouseTarget();
    }
    
    /// <summary>
    /// Called at the end of each frame, caches internal values.
    /// </summary>    
    protected virtual void CacheLastFrameValues() {
        lastMouseDown = mouseDown;
        lastMousePos = mousePos;
        lastMouseRay = mouseRay;
        lastMouseTarget = mouseTarget;        
    }
    
    /// <summary>
    /// Override to add to Awake().
    /// </summary>    
    protected virtual void OnAwake() {}
    
    /// <summary>
    /// Override to add to Start().
    /// </summary>    
    protected virtual void OnStart() {}
    
    /// <summary>
    /// Removes "main" if needed. Overrides must call base!
    /// </summary>    
    protected virtual void OnDestroy() {
        if (main == this) {
            main = null;
        }
    }
    
    /// <summary>
    /// Handles per-frame hook calls. Override to customize.
    /// </summary>    
    protected virtual void OnUpdate() {
        if (mouseDown && !lastMouseDown) {
            OnMouseDown();
        } else if (!mouseDown && lastMouseDown) {
            OnMouseUp();
        }
        
		//always assume mouse has moved -- because the camera itself might have
        OnMouseMove();
        
        if (mouseDown) {
            OnMouseHold();
        }
    }
    
    /// <summary>
    /// Called once when click begins. Calls "MouseDown".
    /// </summary>    
    protected virtual void OnMouseDown() {
        mouseDownTarget = mouseTarget;
        if (mouseDownTarget != null) {
            Send(mouseDownTarget, "MouseDown");
        }
    }
    
    /// <summary>
    /// Called once when click ends. Calls "MouseUp", "MouseRelease".
    /// </summary>    
    protected virtual void OnMouseUp() {
        if (lastMouseTarget != null) {
            Send(lastMouseTarget, "MouseUp");
        }
        if (mouseDownTarget != null && mouseDownTarget == lastMouseTarget) {
            Send(lastMouseTarget, "MouseRelease");
        }
        mouseDownTarget = null;
    }
    
    /// <summary>
    /// Called once per frame when position moves. Calls "MouseOut", "MouseIn".
    /// </summary>    
    protected virtual void OnMouseMove() {
        if (mouseTarget != lastMouseTarget) {
            if (lastMouseTarget != null) {
                Send(lastMouseTarget, "MouseOut");
            }
            if (mouseTarget != null) {
                Send(mouseTarget, "MouseIn");
            }
        }
    }
    
    /// <summary>
    /// Called once per frame while clicking. Calls "MouseHold".
    /// </summary>
    protected virtual void OnMouseHold() {       
        if (mouseTarget != null) {
            Send(mouseTarget, "MouseHold");
        }
    }
    
    /// <summary>
    /// Alias for SendMessage.
    /// </summary>    
    protected void Send(GameObject target, string message) {
        target.SendMessage(message, SendMessageOptions.DontRequireReceiver);
    }
    
    
}