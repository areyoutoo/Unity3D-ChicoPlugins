using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Button. Pairs with CUIPopPanel. When clicked, tells CUIPanelCam to go back one panel.
/// </summary>
[AddComponentMenu("ChicoPlugins/UI/Clickable/Pop Panel")]
public class CUIPopPanel : CUIClickable {
	
	public override void MouseRelease() {
		foreach (CUIPanelCam cam in CUIPanelCam.panelCams) {
			cam.PopPanel();
		}
	}
}
