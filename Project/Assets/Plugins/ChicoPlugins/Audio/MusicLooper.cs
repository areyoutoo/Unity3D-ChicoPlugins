using UnityEngine;
using System.Collections;

/// <summary>
/// MusicPlayer that re-starts its music once finished.
/// </summary>
/// <remarks>
/// Still calls onMusicFinished() after re-starting the music.
/// </remarks>
public class MusicLooper : MusicPlayer {
	protected override void MusicFinished() {
		music.Play();
		base.MusicFinished();
	}
}
