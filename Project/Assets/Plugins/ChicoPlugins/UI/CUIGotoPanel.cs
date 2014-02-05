using UnityEngine;
using System.Collections;

/// <summary>
/// Button. When clicked, tells CUIPanelCam to switch panels WITHOUT a push.
/// </summary>
public class CUIGotoPanel : CUIClickable {
	public string panelName;
	
	CUIPanelCam panelCam;
	
	protected void Start() {
		panelCam = GetPanelCam(transform);
	}
	
	public override void MouseRelease() {
		if (panelCam == null) {
			panelCam = GetPanelCam(transform);
		}
		
		if (panelCam == null) {
			Debug.LogWarning("CUIGotoPanel can't find parent CUIPanelCam!", this);
		} else {
            GotoPanel(panelCam, panelName);
		}
	}
    
    protected virtual void GotoPanel(CUIPanelCam cam, string target) {
        cam.SwitchPanel(panelName);
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
