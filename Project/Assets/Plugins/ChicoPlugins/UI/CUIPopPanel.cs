using UnityEngine;
using System.Collections;

/// <summary>
/// Button. Pairs with CUIPopPanel. When clicked, tells CUIPanelCam to go back one panel.
/// </summary>
public class CUIPopPanel : CUIClickable {
	CUIPanelCam panelCam;
	
	protected void Start() {
		panelCam = GetPanelCam(transform);
	}
	
	public override void MouseRelease() {
		if (panelCam == null) {
			panelCam = GetPanelCam(transform);
		}
		
		if (panelCam == null) {
			Debug.LogWarning("CUIPopPanel can't find parent CUIPanelCam!", this);
		} else {
			panelCam.PopPanel();
		}
	}
	
	protected virtual CUIPanelCam GetPanelCam(Transform root) {
		if (root == null) {
			return null;
		}
		
		CUIPanelCam pc = root.GetComponent<CUIPanelCam>();
		if (pc != null) {
			return pc;
		} else {
			return GetPanelCam(root.parent);
		}
	}
}
