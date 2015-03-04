using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Pool which collects any Transform children.
/// </summary>
/// <remarks>
/// Useful for storing any type of GameObject, including prefab-like objects or
/// components for which you'd rather not create a custom ComponentPool.
/// </remarks>
[AddComponentMenu("ChicoPlugins/Pools/Transform")]
public class TransformPool : ComponentPool<Transform> {
}
