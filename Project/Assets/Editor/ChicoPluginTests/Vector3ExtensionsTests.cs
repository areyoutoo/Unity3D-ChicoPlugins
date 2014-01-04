using UnityEngine;
using System.Collections.Generic;
using UnTest;

[TestSuite]
public class Vector3ExtensionsTests {
	List<Vector3> values;
	
	[TestSetup]
	void TestSetup() {
		const int numValues = 20;
		values = new List<Vector3>(numValues);
		for (int i=0; i<numValues; i++) {
			Vector3 v = Vector3.zero;
			for (int j=0; j<3; j++) {
				v[j] = Random.Range(-10000f, 10000f);
			}
			values.Add(v);
		}
	}
	
	[Test]
	void WithX_Test() {
		int numGoodValues = 0;
		int numTests = 0;
		foreach (var v in values) {
			float f = Random.Range(-1000f, 1000f);
			Vector3 v2 = v.WithX(f);
			numTests += 1;
			numGoodValues += (v2.x == f && v2.y == v.y && v2.z == v.z) ? 1 : 0;
		}
		Assert.IsEqual(numGoodValues, numTests);
	}
	
	[Test]
	void WithY_Test() {
		int numGoodValues = 0;
		int numTests = 0;
		foreach (var v in values) {
			float f = Random.Range(-1000f, 1000f);
			Vector3 v2 = v.WithY(f);
			numTests += 1;
			numGoodValues += (v2.x == v.x && v2.y == f && v2.z == v.z) ? 1 : 0;
		}
		Assert.IsEqual(numGoodValues, numTests);
	}
	
	[Test]
	void WithZ_Test() {
		int numGoodValues = 0;
		int numTests = 0;
		foreach (var v in values) {
			float f = Random.Range(-1000f, 1000f);
			Vector3 v2 = v.WithZ(f);
			numTests += 1;
			numGoodValues += (v2.x == v.x && v2.y == v.y && v2.z == f) ? 1 : 0;
		}
		Assert.IsEqual(numGoodValues, numTests);		
	}
}
