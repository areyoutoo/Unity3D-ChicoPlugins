using UnityEngine;

public abstract class CUIClickable : MonoBehaviour {
    public virtual void MouseDown() {}
    public virtual void MouseUp() {}
    public virtual void MouseRelease() {}
    public virtual void MouseOut() {}
    public virtual void MouseIn() {}
    public virtual void MouseHold() {}
}