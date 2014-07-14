using UnityEngine;
using System.Collections;

/// <summary>
/// Button allows user to toggle a bool.
/// </summary>
[AddComponentMenu("ChicoPlugins/UI/Toggle")]
public class CUIToggle : CUIValue<bool> {
	public GameObject enabledWidget;
	public GameObject disabledWidget;
    
    protected override bool OnSetValue(bool newValue) {
		if (enabledWidget != null) {
			enabledWidget.SetActive(newValue);
		}
		if (disabledWidget != null) {
			disabledWidget.SetActive(!newValue);
		}
        return newValue;
    }
	
	public override void MouseRelease() {
		SetValue(!current);
	}
}
