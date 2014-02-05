using UnityEngine;
using System.Collections;

/// <summary>
/// Switch levels with a loading screen.
/// </summary>
public class LevelLoader : MonoBehaviour {
    /// <summary>
    /// Global hook for level loading with transition scene.
    /// </summary>
    /// <remarks>
    /// Note that both levels must be in your project's build settings.
    /// </remarks>
    /// <param name="target">Name of destination level.</param>
    /// <param name="transition">Optional name of transition level. Defaults to "Loading".</param>
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