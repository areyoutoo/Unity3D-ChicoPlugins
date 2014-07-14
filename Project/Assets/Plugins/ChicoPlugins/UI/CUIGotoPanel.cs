using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Button. When clicked, tells CUIPanelCam to switch panels WITHOUT a push.
/// </summary>
[AddComponentMenu("ChicoPlugins/UI/Clickable/Goto Panel")]
public class CUIGotoPanel : CUIClickable {
	public string panelName;
	
	public override void MouseRelease() {
		
		//TODO
		foreach (CUIPanelCam cam in CUIPanelCam.panelCams) {
			GotoPanel(cam, panelName);
		}
	}
	
	protected virtual void GotoPanel(CUIPanelCam cam, string target) {
		cam.SwitchPanel(target);
	}
}
