using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	public AudioSource music;
	
	/// <summary>
	/// Music audio's volume as it is first played. (read-only)
	/// </summary>
	public float baseVolume { get; protected set; }
	
	/// <summary>
	/// Delegate to be called when music finishes playing.
	/// </summary>
	/// <remarks>
	/// Will be called only once, during the frame after music has finished. 
	/// Not called again until music plays for at least one frame.
	/// </remarks>
	public System.Action onMusicFinished;
	bool firedOnMusicFinished = false;
	
	protected void Awake() {
		if (music == null) {
			if (audio != null) {
				music = audio;
			} else {
				Debug.LogWarning("MusicPlayer with no AudioSource");
				return;
			}
		}
		
		ConfigureSource(music);
		
		float configMulti = GetConfigVolumeMulti();
		SetVolumeMulti(configMulti);
		
		OnAwake();
	}
	
	protected void Update() {
		OnUpdate();
		if (!music.isPlaying) {
			MusicFinished();
		} else {
			firedOnMusicFinished = false;
		}
	}
	
	/// <summary>
	/// Set volume multiplier.
	/// </summary>
	/// <param name='volume'>
	/// Desired music volume; will be modified according to baseVolume.
	/// </param>
	public void SetVolumeMulti(float volume) {
		volume /= baseVolume;
		ApplyVolumeMulti(volume);
	}
	
	/// <summary>
	/// Directly set volume on target AudioSource.
	/// </summary>
	protected virtual void ApplyVolumeMulti(float volume) {
		music.volume = volume;
	}
	
	/// <summary>
	/// If your game configures a music volume, override this to read it.
	/// </summary>
	protected virtual float GetConfigVolumeMulti() {
		return 1f;
	}
	
	protected virtual void MusicFinished() {
		if (firedOnMusicFinished) return;
		
		if (onMusicFinished != null) {
			onMusicFinished();
		}
	}
	
		
	/// <summary>
	/// Optional override. Called at the end of Awake().
	/// </summary>
	protected virtual void OnAwake() {}
	
	/// <summary>
	/// Optional override. Called at the start of Update().
	/// </summary>
	protected virtual void OnUpdate() {}
	
	
	/// <summary>
	/// Configure an AudioSource for use with music. Maybe also useful for UI?
	/// </summary>
	public static void ConfigureSource(AudioSource source) {
		source.ignoreListenerPause = true;
		source.ignoreListenerVolume = true;
		source.bypassEffects = true;
		source.bypassListenerEffects = true;
		source.bypassReverbZones = true;
		
		source.rolloffMode = AudioRolloffMode.Linear;
		source.maxDistance = 10000000f;		
	}
}
