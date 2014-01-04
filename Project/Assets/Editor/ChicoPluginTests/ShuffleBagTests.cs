using UnityEngine;
using System.Collections.Generic;
using UnTest;

[TestSuite]
public class ShuffleBagTests {
	private ShuffleBag<int> bag;
	
	[TestSetup]
	void TestSetup() {
		bag = new ShuffleBag<int>();
	}
	
	[Test]
	void Empty_Returns_Default() {
		int sample = bag.GetNext();
		Assert.Equals(sample, default(int));
	}
	
	[Test]
	void Is_Never_Empty() {
		bag.Add(1);
		
		int numDefaults = 0;
		const int numSamples = 1000;
		for (int i=0; i<numSamples; i++) {
			int sample = bag.GetNext();
			numDefaults += sample == default(int) ? 1 : 0;
		}
		Assert.IsEqual(numDefaults, 0);
	}
	
	[Test]
	void Returns_In_Bag() {
		List<int> values = new List<int>();
		values.Add(-1000);
		values.Add(-1);
		values.Add(10);
		values.Add(17);
		values.Add(32);
		
		bag.AddRange(values);
		
		int numGoodValues = 0;
		const int numSamples = 100;
		for (int i=0; i<numSamples; i++) {
			int sample = bag.GetNext();
			numGoodValues += values.Contains(sample) ? 1 : 0;
		}
		Assert.IsEqual(numGoodValues, numSamples);
	}
	
	[Test]
	void NoRepeat_Until_Refill() {
		//build range of unique values
		const int numSamples = 100;
		List<int> samples = new List<int>(numSamples);
		for (int i=0; i<numSamples; i++) {
			samples.Add(i);
			bag.Add(i);
		}
		
		//request numSamples values
		int numInRange = 0; //all samples should be in range
		int numRepeats = 0; //no sample may repeat
		List<int> returns = new List<int>(numSamples);
		for (int i=0; i<numSamples; i++) {
			int sample = bag.GetNext();
			numInRange += samples.Contains(sample) ? 1 : 0;
			numRepeats += returns.Contains(sample) ? 1 : 0;
			returns.Add(sample);
		}
		Assert.IsEqual(numInRange, numSamples);
		Assert.IsEqual(numRepeats, 0);
		
		//call one more sample, it should be a repeat
		int lastSample = bag.GetNext();
		Assert.IsTrue(samples.Contains(lastSample));
		Assert.IsTrue(returns.Contains(lastSample));
	}
}
