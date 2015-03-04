using UnityEngine;

/// <summary>
/// Handles and caches mouse, world, UI interaction for each frame.
/// </summary>
/// <remarks>
/// - Builds centralized raycasts to avoid each script doing so ad hoc.
/// - Checks if mouse is over UI, world, etc.
/// - Listens for double clicks.
/// - Remembers last click frame, last release frame, etc. (useful for comparisons)
/// </remarks>
public class MouseManager : MonoBehaviour {
	[SerializeField] Camera _worldCam;
	[SerializeField] Camera _uiCam;
	[SerializeField] LayerMask _uiLayer;
	[SerializeField] LayerMask _floorLayer;

	/// <summary>
	/// This mouse manager's world camera. Set via inspector.
	/// </summary>
	public Camera worldCam {
		get { return _worldCam; }
		private set { _worldCam = value; }
	}

	/// <summary>
	/// This mouse manager's UI camera. Set via inspector.
	/// </summary>
	public Camera uiCam {
		get { return _uiCam; }
		private set { _uiCam = value; }
	}

	/// <summary>
	/// Collision layer mask to check UI collisions. Set via inspector.
	/// </summary>
	public LayerMask uiLayer {
		get { return _uiLayer; }
		private set { _uiLayer = value; }
	}

	/// <summary>
	/// Data for the current frame.
	/// </summary>
	public Frame currFrame { get; protected set; }

	/// <summary>
	/// Data for the previous frame.
	/// </summary>
	public Frame prevFrame { get; protected set; }

	/// <summary>
	/// Data for last frame where currFrame.mouseDown was true.
	/// </summary>
	public Frame lastClickFrame { get; protected set; }

	/// <summary>
	/// Data for last frame where currFrame.mouseUp was true.
	/// </summary>
	/// <value>The last release frame.</value>
	public Frame lastReleaseFrame { get; protected set; }

	/// <summary>
	/// Returns true for one frame if the user has just clicked and released.
	/// </summary>
	public bool click { get; protected set; }

	/// <summary>
	/// Returns true for one frame if the user has just tapped twice in quick succession.
	/// </summary>
	public bool doubleClick { get; protected set; }

	public struct Frame {
		/// <summary>
		/// Mouse position for this frame. Measured in screen coords.
		/// </summary>
		public readonly Vector3 mousePos;

		/// <summary>
		/// Value of Input.GetMouseButtonUp for this frame.
		/// </summary>
		public readonly bool mouseUp;

		/// <summary>
		/// Value of Input.GetMouseButtonDown for this frame.
		/// </summary>
		public readonly bool mouseDown;

		/// <summary>
		/// Value of Input.GetMouseButton for this frame.
		/// </summary>
		public readonly bool mouseHeld;

		/// <summary>
		/// Time.time as of this frame. Measured in seconds.
		/// </summary>
		public readonly float time;

		/// <summary>
		/// Mouse ray out of UI camera.
		/// </summary>
		public readonly Ray uiRay;

		/// <summary>
		/// Did uiRay hit anything?
		/// </summary>
		public readonly bool hasUiHit;

		/// <summary>
		/// If hasUiHit is true, contains the hit data for uiRay.
		/// </summary>
		public readonly RaycastHit uiHit;

		/// <summary>
		/// Mouse ray out of world camera.
		/// </summary>
		public readonly Ray worldRay;

		/// <summary>
		/// Did worldRay hit anything?
		/// </summary>
		public readonly bool hasWorldHit;

		/// <summary>
		/// If hasWorldHit is true, contains the hit data for worldRay.
		/// </summary>
		public readonly RaycastHit worldHit;

		/// <summary>
		/// True if either hasUiHit or the collider hit by uiHit has changed since last frame.
		/// </summary>
		public readonly bool uiColliderChanged;

		/// <summary>
		/// True if either hasWorldHit or the collider hit by worldHit has changed since last frame.
		/// </summary>
		public readonly bool worldColliderChanged;

		/// <summary>
		/// Number of seconds since this frame occurred.
		/// </summary>
		/// <value>The time since frame.</value>
		public float timeSinceFrame {
			get { return Time.time - time; }
		}

		public Frame(Frame prevFrame, Camera uiCam, Camera worldCam, LayerMask uiLayer) {
			mousePos = Input.mousePosition;
			mouseDown = Input.GetMouseButtonDown(0);
			mouseUp = Input.GetMouseButtonUp(0);
			mouseHeld = Input.GetMouseButton(0);

			time = Time.time;

			int uiLayerMask = uiLayer.value;

			uiRay = uiCam.ScreenPointToRay(mousePos);
			hasUiHit = Physics.Raycast(uiRay, out uiHit, 10000f, uiLayerMask);
			uiColliderChanged = (hasUiHit != prevFrame.hasUiHit) || (hasUiHit && uiHit.collider != prevFrame.uiHit.collider);

			worldRay = worldCam.ScreenPointToRay(mousePos);
			hasWorldHit = Physics.Raycast(worldRay, out worldHit, 10000f, ~uiLayerMask);
			worldColliderChanged = (hasWorldHit != prevFrame.hasWorldHit) || (hasWorldHit && worldHit.collider != prevFrame.worldHit.collider);
		}
	}

	protected void Awake() {
		if (worldCam == null) {
			Debug.LogError("MouseManager needs world camera", this);
		}

		if (uiCam == null) {
			Debug.LogError("MouseManager needs UI camera", this);
		}

		if (uiLayer.value == 0) {
			Debug.LogWarning("MouseManager has empty UI layer mask (won't hit any UI)", this);
		}
	}

	protected void Update() {
		currFrame = new Frame(prevFrame, uiCam, worldCam, uiLayer);

		click = currFrame.mouseUp && (Time.time - lastClickFrame.time) < 0.3f;
		doubleClick = currFrame.mouseDown && (Time.time - lastReleaseFrame.time) < 0.1f;

		if (currFrame.mouseDown) {
			lastClickFrame = currFrame;
		}

		if (currFrame.mouseUp) {
			lastReleaseFrame = currFrame;
		}
	}

	protected void LateUpdate() {
		prevFrame = currFrame;
	}

	public Vector3 WorldToUIPoint(Vector3 worldPos) {
		Vector3 screenPos = worldCam.WorldToScreenPoint(worldPos).WithZ(0.1f);
		Vector3 uiPos = uiCam.ScreenToWorldPoint(screenPos);
		return uiPos;
	}

	public Vector3 UIToWorldPoint(Vector3 uiPos) {
		Vector3 screenPos = uiCam.WorldToScreenPoint(uiPos);
		Vector3 worldPos = worldCam.ScreenToWorldPoint(screenPos);
		return worldPos;
	}

	public Vector3 ScreenToWorldPoint(Vector3 screenPos) {
		return worldCam.ScreenToWorldPoint(screenPos);
	}

}