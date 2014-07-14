using UnityEngine;
using System.Collections;

[AddComponentMenu("ChicoPlugins/UI/Text/TextButton")]
public class CUITextButton : CUITextWidget {	
	public Color pressColor = Color.red;
	public float pressScale = 1.1f;

	protected ButtonState pressState;

	protected override void OnStart() {
		pressState = new ButtonState(pressColor, transform.localScale * pressScale);
	}
	
	public override void MouseIn() {
//		Debug.Log("MouseIn");
		SwitchState(pressState, 0.1f);
	}
	
	public override void MouseOut() {
//		Debug.Log("MouseOut");
		SwitchState(mainState, 0.2f);
	}
}
