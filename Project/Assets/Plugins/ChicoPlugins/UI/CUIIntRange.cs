using UnityEngine;
using System.Collections;

/// <summary>
/// Two buttons allowing user to pick an int.
/// </summary>
public class CUIIntRange : CUIRange<int> {	
    protected override int ClampValue(int newValue, int min, int max) {
        return Mathf.Clamp(newValue, min, max);
    }
    
    protected override string GetString(int newValue) {
        return newValue.ToString("n");
    }
    
    protected override void Reset() {
        defaultValue = 5;
        min = 1;
        max = 10;
        step = 1;
    }
	
	public override void UpClick() {
		SetValue(current + step);
	}
	
	public override void DownClick() {
		SetValue(current - step);
	}
}
