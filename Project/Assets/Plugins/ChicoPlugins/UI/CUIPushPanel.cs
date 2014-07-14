using UnityEngine;
using System.Collections;

/// <summary>
/// Button. Pairs with CUIPopPanel. When clicked, tells CUIPanelCam to go forward one panel.
/// </summary>
[AddComponentMenu("ChicoPlugins/UI/Clickable/Push Panel")]
public class CUIPushPanel : CUIGotoPanel {
    protected override void GotoPanel(CUIPanelCam cam, string target) {
		cam.PushPanel(target);
    }
}
