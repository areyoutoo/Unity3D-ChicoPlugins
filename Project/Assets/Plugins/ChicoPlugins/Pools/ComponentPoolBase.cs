using UnityEngine;
using System.Collections;

public abstract class ComponentPoolBase : MonoBehaviour {
	[SerializeField] string _id;
	public string id {
		get { return _id; }
	}
	
	[SerializeField] bool _copyOnEmpty = true;
	public bool copyOnEmpty {
		get { return _copyOnEmpty; }
	}
	
	[SerializeField] int _copyOnStart = 0;
	public int copyOnStart {
		get { return _copyOnStart; }
	}
	
	[SerializeField] int _copyRate = 5;
	public int copyRate {
		get { return _copyRate; }
	}
	
	[SerializeField] bool _activateOnGet = true;
	public bool activateOnGet {
		get { return _activateOnGet; }
	}
	
	protected void Reset() {
		_id = name;
	}
}
