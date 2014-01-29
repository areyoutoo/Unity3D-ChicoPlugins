using UnityEngine;

public class SimpleLoadUI : MonoBehaviour {
    public Color backgroundColor = Color.black;
    public Color textColor = Color.blue;
    public string message = "Loading...";
 
    void Start() {
        Camera.main.backgroundColor = backgroundColor;
    }
 
    void OnGUI() {
        //cache and update GUI settings
        Color cachedColor = GUI.contentColor;
        GUI.contentColor = textColor;
 
        //draw label
        float width = 60f;
        float height = 20f;
        float left = Screen.width / 2 - width;
        float top = Screen.height / 2 - height;
        Rect rect = new Rect(left, top, width, height);
        GUI.Label (rect, message);
 
        //restore GUI settings
        GUI.contentColor = cachedColor;
    }
}