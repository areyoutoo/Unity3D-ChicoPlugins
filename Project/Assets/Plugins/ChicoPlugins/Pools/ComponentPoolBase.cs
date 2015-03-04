using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all ComponentPool derivatives.
/// </summary>
/// <remarks>
/// Mainly holds inspector values.
/// </remarks>
public abstract class ComponentPoolBase : MonoBehaviour {
    public bool copyOnEmpty = true;
    public int copyOnStart = 0;
    public int copyRate = 5;
    public bool activateOnGet = true;
    
    public string id {
        get { return gameObject.name; }
    }
    
    protected void Reset() {
        copyOnEmpty = true;
        copyOnStart = 0;
        copyRate = 5;
        activateOnGet = true;
    }
}
