public struct TweenState {
    public float secondsLeft;
    public float seconds;
    
    public float perc {
        get { return 1f - (secondsLeft / seconds); }
    }
    
    public bool isDone {
        get { return secondsLeft < 0f; }
    }
    
    public bool isActive {
        get { return secondsLeft >= 0f; }
    }
    
    public bool Tick(float delta) {
        secondsLeft -= delta;
        return secondsLeft >= 0f;
    }
    
    public void SetPerc(float percent) {
        secondsLeft = seconds * percent;
    }
    
    public void Reset(float duration) {
        secondsLeft = seconds = duration;
    }
    
    public void Stop() {
        secondsLeft = 0f;
    }
    
    public TweenState(float duration) {
        secondsLeft = seconds = duration;
    }
}