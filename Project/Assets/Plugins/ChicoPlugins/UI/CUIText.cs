using UnityEngine;
using System.Collections;

/// <summary>
/// UI widget for text. Extend for specific text classes (ie: TextMesh).
/// </summary>
public abstract class CUIText : CUIValue<string> {
	string lastValue;
    
    protected override string OnSetValue(string newValue) {
        if (newValue != lastValue) {
			lastValue = newValue;
            ApplyText(newValue);
        }
        return newValue;
    }
	
	protected abstract void ApplyText(string newText);
}
