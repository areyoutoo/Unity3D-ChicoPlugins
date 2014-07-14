using UnityEngine;
using System.Collections;

/// <summary>
/// Widget to manage a child CUIText.
/// </summary>
[AddComponentMenu("ChicoPlugins/UI/Clickable/TextWidget")]
public class CUITextWidget : CUIClickable {
	protected CUIText text { get; private set; }

	public string message {
		get { return text.message; }
		set { text.message = value; }
	}

	public Color color {
		get { return text.color; }
		set { text.color = value; }
	}

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
	
	protected ButtonState mainState;

	protected void Awake() {
		text = GetComponentInChildren<CUIText>();
		if (text == null) {
			Debug.LogWarning("CUITextButton without TextMesh!", this);
			return;
		}
		
		tween = new TweenState(0f);
		mainState = new ButtonState(text.color, transform.localScale);
		lastState = state = mainState;

		OnStart();
	}

	
	protected virtual void Update() {
		if (tween.Tick(Time.deltaTime)) {
			text.color = Color.Lerp(lastState.color, state.color, tween.perc);
			transform.localScale = Vector3.Lerp(lastState.size, state.size, tween.perc);
		}
	}

	public void PulseScale(float scale, float duration) {
		Pulse(mainState.color, mainState.size * scale, duration);
	}

	public void PulseColor(Color color, float duration) {
		Pulse(color, mainState.size, duration);
	}

	public void Pulse(Color color, Vector3 scale, float duration) {
		ButtonState newState = new ButtonState(color, scale);
		SnapState(newState);
		SwitchState(mainState, duration);
	}
	
	protected void SwitchState(ButtonState newState, float duration) {
		lastState = new ButtonState(text.color, transform.localScale);
		state = newState;
		tween.Reset(duration);
	}

	protected void SnapState(ButtonState newState) {
		lastState = state;
		state = newState;
		tween.Stop();
		text.color = newState.color;
		transform.localScale = newState.size;
	}

	protected virtual void OnStart() {}
}
