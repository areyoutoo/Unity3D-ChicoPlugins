using UnityEngine;

public class CUIClickDelegate : CUIClickable {
    public System.Action OnMouseDown;
    public System.Action OnMouseUp;
    public System.Action OnMouseRelease;
    public System.Action OnMouseOut;
    public System.Action OnMouseIn;
    public System.Action OnMouseHold;

    public override void MouseDown() {
        if (OnMouseDown != null) {
            OnMouseDown();
        }
    }
    
    public override void MouseUp() {
        if (OnMouseUp != null) {
            OnMouseUp();
        }    
    }
    
    public override void MouseRelease() {
        if (OnMouseRelease != null) {
            OnMouseRelease();
        }    
    }
    
    public override void MouseOut() {
        if (OnMouseOut != null) {
            OnMouseOut();
        }    
    }
    
    public override void MouseIn() {
        if (OnMouseIn != null) {
            OnMouseIn();
        }    
    }
    
    public override void MouseHold() {
        if (OnMouseHold != null) {
            OnMouseHold();
        }    
    }
}