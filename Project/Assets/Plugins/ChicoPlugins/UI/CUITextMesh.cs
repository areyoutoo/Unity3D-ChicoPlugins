using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class CUITextMesh : CUIText {
	protected TextMesh tm;
	
	protected void Start() {
		tm = GetComponent<TextMesh>();
	}
	
	protected override void ApplyMessage(string newMessage) {
		tm.text = newMessage;
	}
	
	protected override void ApplyColor(Color newColor) {
		tm.color = newColor;
	}
}
