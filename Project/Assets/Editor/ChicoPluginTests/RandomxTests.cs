using UnityEngine;
using System.Collections.Generic;
using UnTest;

[TestSuite]
public class RandomxTests {

	[Test]
	void Bounds_In_Bounds() {
		List<Vector3> sizes = new List<Vector3>(new Vector3[]{
			Vector3.one,
			Vector3.one * 20f,
			new Vector3(100f, 100f, 200f),
		});
		
		List<Vector3> centers = new List<Vector3>(sizes);
		foreach (var size in sizes) {
			centers.AddRange(new Vector3[]{
				new Vector3(size.x, size.y, size.z),
				new Vector3(-size.x, -size.y, -size.z),
				new Vector3(size.x, -size.y, -size.z),
			});
		}
		
		int numInBounds = 0;
		int numTests = 0;
		foreach (var center in centers) {
			foreach (var size in sizes) {
				Bounds b = new Bounds(center, size);
				
				for (int i=0; i<10; i++) {
					Vector3 v = Randomx.InBounds(b);
					numTests += 1;
					numInBounds += b.Contains(v) ? 1 : 0;
				}
			}
		}
		
		Assert.IsEqual(numInBounds, numTests);
	}
	
	[Test]
	void OnLine_On_Line() {
		const int numPoints = 20;
		List<Vector3> points = new List<Vector3>(numPoints);
		for (int i=0; i<numPoints; i++) {
			points.Add(Random.insideUnitSphere * Random.Range(1f, 1000f));
		}
		
		int numOnLine = 0;
		int numTests = 0;
		foreach (var p1 in points) {
			foreach (var p2 in points) {
				const int numSamples = 4;
				for (int i=0; i<numSamples; i++) {
					Vector3 v = Randomx.OnLine(p1, p2);
					
					Vector3 dirP1toV = (v - p1).normalized;
					Vector3 dirP1toP2 = (p2 - p1).normalized;
					
					Vector3 dirP2toV = (v - p2).normalized;
					Vector3 dirP2toP1 = (p1 - p2).normalized;
					
					numTests += 1;
					if (dirP1toV == dirP1toP2 || dirP2toV == dirP2toP1) { //if V is very close to p1 or p2, this gets funny, so compare from both sides
						numOnLine += 1;
					}
				}
			}
		}
		
		Assert.IsEqual(numOnLine, numTests);
	}
}
