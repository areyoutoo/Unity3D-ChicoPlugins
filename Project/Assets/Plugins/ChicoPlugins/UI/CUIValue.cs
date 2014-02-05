using UnityEngine;
using System.Collections;

/// <summary>
/// UI widget representing a value that can be changed (int, float, string, etc.)
/// </summary>
public abstract class CUIValue<T> : CUIClickable {
    public T defaultValue = default(T);
    
    /// <summary>
    /// Delegate to be called by SetValue.
    /// </summary>
    public System.Action<T> onChange;
    
    /// <summary>
    /// Widget's current value (read-only).
    /// </summary>
    public T current { get; private set; }
    
    protected void Start() {
        SetValue(defaultValue, false);
        OnStart();
    }
    
    /// <summary>
    /// Call to change the widget's value.
    /// </summary>
    /// <param name="newValue">Input value.</param>
    /// <param name="fireOnChange">Call the onChange delegate?</param>
    public void SetValue(T newValue, bool fireOnChange=true) {
        current = OnSetValue(newValue);
        if (fireOnChange && onChange != null) {
            onChange(current);
        }
    }
    
    /// <summary>
    /// Called by SetValue. Gives us a chance to handle or modify the input.
    /// </summary>
    /// <param name="newValue">Input value.</param>
    /// <returns>newValue (possibly modified)</returns>
    protected virtual T OnSetValue(T newValue) {
        return newValue;
    }
    
    /// <summary>
    /// Called at the end of Start().
    /// </summary>
    protected virtual void OnStart() {}
    
    /// <summary>
    /// Called when the component is attached; override to set default values.
    /// </summary>    
    protected virtual void Reset() {}
}