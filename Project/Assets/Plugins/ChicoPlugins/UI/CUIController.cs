using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class CUIController : MonoBehaviour {
    public Camera uiCamera { get; private set; }
    public int uiLayerMask { get; private set; }
    
    public GameObject mouseTarget { get; private set; }
    public GameObject lastMouseTarget { get; private set; }
    
    public Vector3 mousePos { get; private set; }
    public Vector3 lastMousePos { get; private set; }
    
    public Ray mouseRay { get; private set; }
    public Ray lastMouseRay { get; private set; }
    
    public bool mouseDown { get; private set; }
    public bool lastMouseDown { get; private set; }
    
    public GameObject mouseDownTarget { get; private set; }
    
    public bool isMain = false;
	
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
    
    protected virtual bool GetMouseDown() {
        #if UNITY_STANDALONE || UNITY_EDITOR
        return Input.GetMouseButton(0);
        #else
        return Input.touchCount > 0;
        #endif
    }
    
    protected virtual Vector3 GetMousePosition() {
        Vector3 mp = Input.mousePosition;
        
        #if !UNITY_STANDALONE
            #if UNITY_EDITOR
                if (!Input.GetMouseButton(0)) { mp = Vector3.zero; }
            #else
                if (Input.touchCount < 1) { mp = Vector3.zero; }
            #endif
        #endif
        
        return mp;
    }
    
    protected virtual Ray GetMouseRay() {
        return uiCamera.ScreenPointToRay(mousePos);
    }
    
    protected virtual GameObject GetMouseTarget() {
        GameObject target = null;
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, 100000f, uiLayerMask)) {
            target = hit.collider.gameObject;
        }
        return target;
    }
    
    protected virtual void UpdateFrameValues() {
        mouseDown = GetMouseDown();
        mousePos = GetMousePosition();
        mouseRay = GetMouseRay();
        mouseTarget = GetMouseTarget();
    }
    
    protected virtual void CacheLastFrameValues() {
        lastMouseDown = mouseDown;
        lastMousePos = mousePos;
        lastMouseRay = mouseRay;
        lastMouseTarget = mouseTarget;        
    }
    
    protected virtual void OnAwake() {}
    protected virtual void OnStart() {}
    
    protected virtual void OnDestroy() {
        if (main == this) {
            main = null;
        }
    }
    
    protected virtual void OnUpdate() {
        if (mouseDown && !lastMouseDown) {
            OnMouseDown();
        } else if (!mouseDown && lastMouseDown) {
            OnMouseUp();
        } else if (mouseDown) {
            OnMouseHold();
        }
    }
    
    protected virtual void OnMouseDown() {
        mouseDownTarget = mouseTarget;
        if (mouseDownTarget != null) {
            Send(mouseDownTarget, "MouseDown");
        }
    }
    
    protected virtual void OnMouseUp() {
        if (lastMouseTarget != null) {
            Send(lastMouseTarget, "MouseUp");
        }
        if (mouseDownTarget != null && mouseDownTarget == lastMouseTarget) {
            Send(lastMouseTarget, "MouseRelease");
        }
        mouseDownTarget = null;
    }
    
    protected virtual void OnMouseHold() {
        if (mouseTarget != lastMouseTarget) {
            if (lastMouseTarget != null) {
                Send(lastMouseTarget, "MouseOut");
            }
            if (mouseTarget != null) {
                Send(mouseTarget, "MouseIn");
            }
        }
        
        if (mouseTarget != null) {
            Send(mouseTarget, "MouseHold");
        }
    }
    
    protected void Send(GameObject target, string message) {
        target.SendMessage(message, SendMessageOptions.DontRequireReceiver);
    }
    
    
}