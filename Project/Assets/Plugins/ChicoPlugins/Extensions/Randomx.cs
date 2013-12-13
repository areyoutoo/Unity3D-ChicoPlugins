using UnityEngine;
using System.Collections;

public static class Randomx {
	
	/// <summary>
	/// Returns a random point within some bounds.
	/// </summary>
	public static Vector3 InBounds(Bounds b) {
		Vector3 v = Vector3.zero;
		for (int i=0; i<3; i++) {
			v[i] = Random.Range(b.min[i], b.max[i]);
		}
		return v;
	}
	
	/// <summary>
	/// Returns a random point between 'from' and 'to'.
	/// </summary>
	public static Vector3 OnLine(Vector3 from, Vector3 to) {
		return Vector3.Lerp(from, to, Random.value);
	}
	
	/// <summary>
	/// Returns a number within [-max,-min] or [min,max]
	/// </summary>
	public static float AbsRange(float min, float max) {
		float f = Random.Range(min, max);
		return Random.value < 0.5f ? f : -f;
	}

	/// <summary>
	/// Returns a number within [-max,-min] or [min,max]
	/// </summary>
	public static int AbsRange(int min, int max) {
		int i = Random.Range(min, max);
		return Random.value < 0.5f ? i : -i;
	}
	
	/// <summary>
	/// Returns true 50% of the time.
	/// </summary>
	public static bool CoinToss() {
		return Random.value < 0.5f;
	}
	
	/// <summary>
	/// Returns true some of the time.
	/// </summary>
	/// <param name='truePerc'>
	/// How often to return true? 0 is never, 1 is 100%.
	/// </param>
	public static bool CoinToss(float truePerc) {
		return Random.value < truePerc;
	}
}
