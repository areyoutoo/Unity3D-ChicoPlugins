using UnityEngine;
using System.Collections;

/// <summary>
/// UI widget for text. Extend for specific text classes (ie: TextMesh).
/// </summary>
public abstract class CUIText : MonoBehaviour {
	string lastMessage;
	Color lastColor;

	protected bool initialized { get; private set; }
	
	public string message {
		get {
			Init();
			return lastMessage;
		}
		set {
			Init();
			if (value != lastMessage) {
				lastMessage = value;
				ApplyMessage(value);
			}
		}
	}
	
	public Color color {
		get {
			Init();
			return lastColor;
		}
		set {
			Init();
			if (value != lastColor) {
				lastColor = value;
				ApplyColor(value);
			}
		}
	}

	protected void Init() {
		if (initialized) return;

		OnInit();

		lastMessage = GetMessage();
		lastColor = GetColor();

		initialized = true;
	}

	protected virtual void OnInit() {}
    
	protected abstract void ApplyMessage(string newMessage);
	protected abstract void ApplyColor(Color newColor);

	protected abstract string GetMessage();
	protected abstract Color GetColor();
}
