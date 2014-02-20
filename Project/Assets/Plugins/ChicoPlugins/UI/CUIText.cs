using UnityEngine;
using System.Collections;

/// <summary>
/// UI widget for text. Extend for specific text classes (ie: TextMesh).
/// </summary>
public abstract class CUIText : MonoBehaviour {
	string lastMessage;
	Color lastColor;
	
	public string message {
		get {
			return lastMessage;
		}
		set {
			if (value != lastMessage) {
				lastMessage = value;
				ApplyMessage(value);
			}
		}
	}
	
	public Color color {
		get {
			return lastColor;
		}
		set {
			if (value != lastColor) {
				lastColor = value;
				ApplyColor(value);
			}
		}
	}
    
	protected abstract void ApplyMessage(string newMessage);
	protected abstract void ApplyColor(Color newColor);
}
