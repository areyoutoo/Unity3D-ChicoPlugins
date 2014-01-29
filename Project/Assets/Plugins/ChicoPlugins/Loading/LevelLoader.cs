using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {
    public static void Load(string name) {
        GameObject go = new GameObject("LevelLoader");
        LevelLoader instance = go.AddComponent<LevelLoader>();
        instance.StartCoroutine(instance.InnerLoad(name));
    }
 
    IEnumerator InnerLoad(string name) {
        //load transition scene
        Object.DontDestroyOnLoad(this.gameObject);
        Application.LoadLevel("Loading");
 
        //wait one frame (for rendering, etc.)
        yield return null;
 
        //load the target scene
        Application.LoadLevel(name);
        Destroy(this.gameObject);
    }
}