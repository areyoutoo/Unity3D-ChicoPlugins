using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attach to a UI camera so that it can scroll between various "panels".
/// </summary>
/// <remarks>
///
/// <para>
/// At startup, this script recursively searches its children for CUIPanel
/// components, registering them as it goes. You can later tell it to "switch"
/// the camera between those panels.
/// </para>
///
/// <para>
/// A CUIPanel should not contain another CUIPanel. Such panels will
/// be ignored by the search.
/// </para>
///
/// <para>
/// CUIPanel does not apply any restriction as far as what the user can click.
/// It is a location in space, and nothing more.
/// </para>
///
/// <para>
/// Generally, you'll control this by calling PushPanel (advances one panel) and
/// PopPanel (goes back one panel). These functions can easily be controlled by
/// the CUIPushPanel and CUIPopPanel widgets.
/// </para>
///
/// <para>
/// Highly suggest you read the comments on handleBackButton and quitOnLastBack.
/// </para>
///
/// <para>
/// Note this is technically a ChaseCam (which is a TweenCam), so you can access
/// all those functions if you need to.
/// </para>
///
/// </remarks>
[RequireComponent(typeof(Camera))]
public class CUIPanelCam : ChaseCam {
	Dictionary<string, CUIPanel> panels = new Dictionary<string, CUIPanel>();
    
    List<CUIPanel> backPanels = new List<CUIPanel>();
    
    /// <summary>
    /// At start, snap to this panel (inspector).
    /// </summary>
    public string defaultPanel = "main";

    /// <summary>
    /// Handle "back" or "escape" keys by calling PopPanel? (inspector)
    /// </summary>    
    public bool handleBackButton = true;

    /// <summary>
    /// When calling PopPanel with no more panels, should the game quit? (inspector)
    /// </summary>
    public bool quitOnLastBack = true;
    
    /// <summary>
    /// Delegate to call when we PopPanel with no more panels. (inspector)
    /// </summary>
    public System.Action onLastBack;
	
	protected override void OnStart() {
		base.OnStart();
		RegisterPanels(target);
        
        if (panels.ContainsKey(defaultPanel)) {
            SnapTarget(panels[defaultPanel].transform);
        }
	}
	
    /// <summary>
    /// Add a CUIPanel to the set. Note that children are automatically searched at startup.
    /// </summary>
    /// <remarks>
    /// Panel names must be unique within a set; if a panel has no name set, its
    /// GameObject name is used instead.
    /// </remarks>
	public void AddPanel(CUIPanel panel) {
        //use panel's name (or its GameObject name)
        string panelName = panel.panelName;
        if (string.IsNullOrEmpty(panelName)) {
            panelName = panel.name;
        }
        
        //names must be unique
		if (panels.ContainsKey(panelName)) {
			Debug.LogWarning("Duplicate CUIPanel name '"+panel.panelName+"'", this);
		} else {
			panels.Add(panelName, panel);
		}
	}
	
    /// <summary>
    /// Remove a CUIPanel from the set.
    /// </summary>    
	public void RemovePanel(CUIPanel panel) {
		panels.Remove(panel.panelName);
        
        //edge case: if panel being removed was on the stack, we must remove it
        while (backPanels.Remove(panel));
	}
	
    /// <summary>
    /// Move the camera to a CUIPanel (by name).
    /// </summary>    
	public void SwitchPanel(string panelName) {
        CUIPanel p;
		if (panels.TryGetValue(panelName, out p)) {
			SwitchTarget(p.transform);
		} else {
			Debug.LogWarning("CUIPanelCam has no panel named '"+panelName+"'", this);
		}
	}
    
    /// <summary>
    /// Move the camera to a CUIPanel, add it to the list of "back" panels.
    /// </summary>    
    public void PushPanel(string panelName) {
        CUIPanel p;
        if (panels.TryGetValue(panelName, out p)) {
            SwitchTarget(p.transform);
            backPanels.Add(p);
        } else {
			Debug.LogWarning("CUIPanelCam has no panel named '"+panelName+"'", this);
		}
    }
    
    /// <summary>
    /// Move the camera back one panel. If no more panels, call delegate and/or quit.
    /// </summary>
    /// <remarks>
    /// If the backPanels list is empty, we will call the onLastBackDelegate; if
    /// quitOnLastBack is true, we will then ALSO call Application.Quit.
    /// </remarks>
    public void PopPanel() {
        int index = backPanels.Count - 1;
        if (index >= 0) {
            CUIPanel p = backPanels[index];
            backPanels.RemoveAt(index);
            SwitchTarget(p.transform);
        } else {
            if (onLastBack != null) {
                onLastBack();
            }
            if (quitOnLastBack) {
                Application.Quit();
            }
        }
    }
	
	void RegisterPanels(Transform root) {
		CUIPanel panel = root.GetComponent<CUIPanel>();
		if (panel != null) {
			AddPanel(panel);
		} else {
			foreach (Transform child in root) {
				RegisterPanels(child);
			}
		}
	}
    
    protected override CamState GetStateDefault(CamState state) {
        state.pos = target.position - target.forward * 10f;
        state.target = target.position;
        return state;
    }
    
    protected override void OnUpdate() {
        base.OnUpdate();
        if (handleBackButton && Input.GetKeyDown(KeyCode.Escape)) {
            PopPanel();
        }
    }
}
