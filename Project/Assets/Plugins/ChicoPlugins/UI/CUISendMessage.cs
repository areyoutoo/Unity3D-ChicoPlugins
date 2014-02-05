using UnityEngine;
using System.Collections;

/// <summary>
/// Pass clicks to other GameObjects via SendMessage.
/// </summary>
public class CUISendMessage : CUIClickable {
	public GameObject target;
	
	public string mouseDownMessage;
	public string mouseUpMessage;
	public string mouseReleaseMessage;
	public string mouseOutMessage;
	public string mouseInMessage;
	public string mouseHoldMessage;
	
    public override void MouseDown() {
		if (!string.IsNullOrEmpty(mouseDownMessage)) Send(mouseDownMessage);
	}
	
    public override void MouseUp() {
		if (!string.IsNullOrEmpty(mouseUpMessage)) Send(mouseUpMessage);
	}
	
    public override void MouseRelease() {
		if (!string.IsNullOrEmpty(mouseReleaseMessage)) Send(mouseReleaseMessage);
	}
	
    public override void MouseOut() {
		if (!string.IsNullOrEmpty(mouseOutMessage)) Send(mouseOutMessage);
	}
	
    public override void MouseIn() {
		if (!string.IsNullOrEmpty(mouseInMessage)) Send(mouseInMessage);
	}
	
    public override void MouseHold() {
		if (!string.IsNullOrEmpty(mouseHoldMessage)) Send(mouseHoldMessage);
	}
	
    protected void Send(string message) {
        target.SendMessage(message, SendMessageOptions.DontRequireReceiver);
    }	
}
