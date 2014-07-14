using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
[AddComponentMenu("ChicoPlugins/UI/Text/TextMesh")]
public class CUITextMesh : CUIText {
	protected TextMesh tm;
	
	protected override void OnInit() {
		if (initialized) return;

		tm = GetComponent<TextMesh>();
	}
	
	protected override void ApplyMessage(string newMessage) {
		tm.text = newMessage;
	}
	
	protected override void ApplyColor(Color newColor) {
		tm.color = newColor;
	}

	protected override string GetMessage() {
		return tm.text;
	}

	protected override Color GetColor() {
		return tm.color;
	}
}
