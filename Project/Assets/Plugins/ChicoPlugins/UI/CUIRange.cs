using UnityEngine;
using System.Collections;

/// <summary>
/// UI widget representing a range of values (int picker, float picker, etc.)
/// </summary>
public abstract class CUIRange<T> : CUIValue<T> {
    /// <summary>
    /// Button to call DownClick.
    /// </summary>    
	public CUIClickDelegate downButton;
    
    /// <summary>
    /// Button to call UpClick.
    /// </summary>    
	public CUIClickDelegate upButton;
	
    /// <summary>
    /// Range should disallow values below this (inspector).
    /// </summary>    
	public T min;
    
    /// <summary>
    /// Range should disallow values above this (inspector).
    /// </summary>    
	public T max;
    
    /// <summary>
    /// On button press, range should change by this much (inspector).
    /// </summary>
	public T step;
	
	protected override void OnStart() {
		if (downButton != null) {
			downButton.OnMouseRelease = () => this.DownClick();
		} else {
			Debug.LogWarning("CUIRange missing downButton", this);
		}
		if (upButton != null) {
			upButton.OnMouseRelease = () => this.UpClick();
		} else {
			Debug.LogWarning("CUIRange missing upButton", this);
		}
	}
	
    protected override T OnSetValue(T newValue) {
        newValue = ClampValue(newValue, min, max);
        return newValue;
    }
    
    /// <summary>
    /// Override to provide type-specific clamp function.
    /// </summary>    
    protected abstract T ClampValue(T newValue, T min, T max);
	
    /// <summary>
    /// Called when "up" button is pressed.
    /// </summary>    
	public abstract void UpClick();
	
    /// <summary>
    /// Called when "down" button is pressed.
    /// </summary>    
	public abstract void DownClick();
}