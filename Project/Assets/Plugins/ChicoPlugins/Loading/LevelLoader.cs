using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {
    public static void Load(string target, string transition="Loading") {
        GameObject go = new GameObject("LevelLoader");
        LevelLoader instance = go.AddComponent<LevelLoader>();
        instance.StartCoroutine(instance.InnerLoad(target, transition));
    }
 
    IEnumerator InnerLoad(string target, string transition) {
        //load transition scene
        Object.DontDestroyOnLoad(this.gameObject);
        Application.LoadLevel(transition);
 
        //wait one frame (for rendering, etc.)
        yield return null;
 
        //load the target scene
        Application.LoadLevel(target);
        Destroy(this.gameObject);
    }
}