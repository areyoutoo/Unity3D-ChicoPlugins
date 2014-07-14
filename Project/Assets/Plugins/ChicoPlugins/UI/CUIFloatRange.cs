using UnityEngine;
using System.Collections;

[AddComponentMenu("ChicoPlugins/UI/Range/Float")]
public class CUIFloatRange : CUIRange<float> {	
    protected override float ClampValue(float newValue, float min, float max) {
        return Mathf.Clamp(newValue, min, max);
    }
    
    protected override string GetString(float newValue) {
        string format = "n0";
        if (step <= 0.001f) {
            format = "n3";
        } else if (step <= 0.01f) {
            format = "n2";
        } else if (step <= 0.1f) {
            format = "n1";
        }
        return newValue.ToString(format);
    }
    
    protected override void Reset() {
        defaultValue = 5f;
        min = 0f;
        max = 10f;
        step = 0.5f;
    }
	
	public override void UpClick() {
		SetValue(current + step);
	}
	
	public override void DownClick() {
		SetValue(current - step);
	}
}
