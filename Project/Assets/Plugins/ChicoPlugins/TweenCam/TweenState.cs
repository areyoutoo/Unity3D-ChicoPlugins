/// <summary>
/// Simple lerp timer.
/// </summary>
public struct TweenState {
    public float secondsLeft;
    public float seconds;
    
    /// <summary>
    /// Timer's progress from 0 (start) to 1 (finish).
    /// </summary>        
    public float perc {
        get { return 1f - (secondsLeft / seconds); }
    }
    
    /// <summary>
    /// Is the timer finished?
    /// </summary>        
    public bool isDone {
        get { return secondsLeft < 0f; }
    }
    
    /// <summary>
    /// Is the timer still running?
    /// </summary>        
    public bool isActive {
        get { return secondsLeft >= 0f; }
    }
    
    /// <summary>
    /// Call once per frame. Advance timer by delta seconds (usually Time.deltaTime).
    /// </summary>
    /// <returns>
    /// True if timer has finished.
    /// </returns>
    public bool Tick(float delta) {
        secondsLeft -= delta;
        return secondsLeft >= 0f;
    }
    
    /// <summary>
    /// Restart the timer with current duration.
    /// </summary>        
    public void Reset() {
        Reset(seconds);
    }
    
    /// <summary>
    /// Restart the timer with given duration.
    /// </summary>
    public void Reset(float duration) {
        secondsLeft = seconds = duration;
    }
    
    /// <summary>
    /// Force the timer to finish.
    /// </summary>        
    public void Stop() {
        secondsLeft = 0f;
    }
    
    public TweenState(float duration) {
        secondsLeft = seconds = duration;
    }
}