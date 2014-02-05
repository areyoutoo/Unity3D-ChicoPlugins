using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all ComponentPool derivatives.
/// </summary>
/// <remarks>
/// Mainly holds inspector values.
/// </remarks>
public abstract class ComponentPoolBase : MonoBehaviour {
	[SerializeField] string _id;
    [SerializeField] bool _copyOnEmpty = true;
    [SerializeField] int _copyOnStart = 0;
    [SerializeField] int _copyRate = 5;
    [SerializeField] bool _activateOnGet = true;
    
    /// <summary>
    /// Unique identifier for the pool. (inspector)
    /// </summary>
    /// <remarks>
    /// Defaults to the name of the attached GameObject.
    /// </remarks>    
	public string id {
		get { return _id; }
	}
	
    /// <summary>
    /// When the pool is exhausted, should it Instantiate() new members? (inspector)
    /// </summary>
    /// <remarks>
    /// If copyOnEmpty is true, the pool will store its first member "on ice" as
    /// a master copy for extra members.
    /// </remarks>        
	public bool copyOnEmpty {
		get { return _copyOnEmpty; }
	}
    
    /// <summary>
    /// Should the pool Instantiate() new members when gameplay starts? (inspector)
    /// </summary>
    /// <remarks>
    /// Useful to keep your scenes simple and uncluttered.
    /// </remarks>    
	public int copyOnStart {
		get { return _copyOnStart; }
	}
    
    /// <summary>
    /// Max Instantiate() calls per frame? (inspector)
    /// </summary>
    /// <remarks>
    /// Referenced only if copyOnStart or copyOnEmpty are true.
    ///
    /// Note this value will appear to be ignored in one circumstance: if you
    /// ask for more than copyRate items in a single frame and copyOnEmpty is
    /// true, the pool will continue to clone one new member for each additional
    /// request.
    /// </remarks>        
	public int copyRate {
		get { return _copyRate; }
	}
    
    /// <summary>
    /// Should we SetActive(true) any pool members right before giving them to you?
    /// </summary>
    /// <remarks>
    /// Pool members will always be disabled as they are added to the pool; this
    /// flag determines whether they are enabled as they leave the pool.
    /// </remarks>    
	public bool activateOnGet {
		get { return _activateOnGet; }
	}
	
	protected void Reset() {
		_id = name;
	}
}
