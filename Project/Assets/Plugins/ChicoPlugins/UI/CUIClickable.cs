using UnityEngine;
using System.Collections;

/// <summary>
/// UI element that can be clicked.
/// </summary>
/// <remarks>
/// See CUIController for hook descriptions.
/// </remarks>
public abstract class CUIClickable : MonoBehaviour {
    public virtual void MouseDown() {}
    public virtual void MouseUp() {}
    public virtual void MouseRelease() {}
    public virtual void MouseOut() {}
    public virtual void MouseIn() {}
    public virtual void MouseHold() {}
}