using UnityEngine;
using System.Collections;

public class CUITextButton : CUIClickable {	
	CUIText text;
	
	public struct ButtonState {
		public Color color;
		public Vector3 size;
		
		public ButtonState(Color color, Vector3 size) {
			this.color = color;
			this.size = size;
		}
	}
	
	TweenState tween;
	ButtonState state;
	ButtonState lastState;
	
	ButtonState mainState;
	ButtonState pressState;
	
	public Color mainColor = Color.black;
	public Color pressColor = Color.red;
	
	public float pressScale = 1.1f;
	
	protected void Start() {
		text = GetComponentInChildren<CUIText>();
		if (text == null) {
			Debug.LogWarning("CUITextButton without TextMesh!", this);
			return;
		}
		
		tween = new TweenState(0f);
		mainState = new ButtonState(mainColor, transform.localScale);
		pressState = new ButtonState(pressColor, transform.localScale * pressScale);
		
		lastState = state = mainState;
	}
	
	public override void MouseIn() {
		Debug.Log("MouseIn");
		SwitchState(pressState, 0.1f);
	}
	
	public override void MouseOut() {
		Debug.Log("MouseOut");
		SwitchState(mainState, 0.2f);
	}
	
	protected void Update() {
		if (tween.Tick(Time.deltaTime)) {
			text.color = Color.Lerp(lastState.color, state.color, tween.perc);
			transform.localScale = Vector3.Lerp(lastState.size, state.size, tween.perc);
		}
	}
	
	void SwitchState(ButtonState newState, float duration) {
		lastState = new ButtonState(text.color, transform.localScale);
		state = newState;
		tween.Reset(duration);
	}
}
