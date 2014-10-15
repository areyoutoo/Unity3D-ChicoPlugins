ChicoPlugins
============

Suite of helpful Unity3D code files.

This project is a collection of files and modules that I've found useful for multiple projects.


Installation
----

Many files, functions, or modules may be useful individually. Those, you can download or copy directly.

To deploy the plugins in your project:

* Clone the repo to your machine
* Copy the contents of `Project/Assets/Plugins` into your project's `Assets/Plugins` folder
* Future releases may include .unitypackage archives to make this simpler

To get a full development build:

* Clone the repo to your machine
* The `Project` folder contains everything you need to create a Unity project
* In Unity: `File` > `New Project` > `Browse...` > Select the cloned "Project" folder
* Unity will automatically create metadata files (asset database, project settings, solutions, etc.)


Bags module
----

Helper classes to manage the storage and randomized retrieval of values. [Managed randomness](http://gamedevelopment.tutsplus.com/tutorials/shuffle-bags-making-random-feel-more-random--gamedev-1249) sometimes [feels more authentic](http://seanmonstar.com/post/708989796/a-less-random-generator) for gameplay.

All bags provide a common `GetNext()` call. Most of them provide a simple `Add(T)` call, or can be populated during construction by passing any IEnumerable reference.

<dl>
<dt>RandomBag</dt>
<dd>Pick items with uniform randomness.</dd>

<dt>ShuffleBag</dt>
<dd>Pick items with uniform randomness; avoids repeating items until the bag has been completely used (and automatically refilled).</dd>

<dt>WeightedBag</dt>
<dd>Pick items with weighted randomness.</dd>
</dl>


ComponentPools module
----

Instantiate() and Destroy() can be expensive calls; it is often more performant to recycle GameObjects using an [object pool](http://answers.unity3d.com/questions/196413/gameobject-pool-to-avoid-performance-issues-in-ins.html) pattern.

Our pools focus on components and the scene hierarchy. With a scene hierarchy like this:

    Pool (has ParticlePool named "Explosion")
        Pooled object (has ParticleSystem)
        Pooled object (has ParticleSystem)

It's easy to find and recycle those particles:

    PoolManager.Get<ParticlePool>("Explosion").GetNextAt(Vector3.zero);
    
Or:

    ParticleSystem ps;
    ParticlePool.TryGetNextAt("Explosion", Vector3.zero, out ps);

* The pool will move a particle system to the requested location.
* By default, the particles will play immediately.
* By default, new particle systems will be spawned when the pool is empty.
* Finished particles will be reclaimed automatically.

Pool structure is managed at runtime, using the scene hierarchy.

<dl>
<dt>PoolManager</dt>
<dd>Static class to easily find and use pools.</dd>

<dt>ParticlePool</dt>
<dd>Pool to manage ParticleSystem components.</dd>

<dt>AudioPool</dt>
<dd>Pool to manage AudioSource components.</dd>

<dt>TransformPool</dt>
<dd>Every GameObject has a transform, making this pool extremely versatile.</dd>
</dl>

It's easy to manage other components, including your own custom components, by creating a new class inheriting from `ComponentPool<T>`.


Loading module
----

Simple loading screen support, similar to [this blog post](http://chicounity3d.wordpress.com/2014/01/25/loading-screen-tutorial/).


TweenCam module
----

TweenCam tracks its state with two points in space: where is the camera, and what is it looking at? You can provide your own functions to provide that state, once per frame, and cleanly tween between output from two such functions.

For advanced use, TweenCam also allows you to control the camera's FOV/size and "up" direction.


UI module
----

This is a work-in-progress module, an attempt to create a bare-bones UI system.


Extensions module
----

This module provides a few [extension methods](http://www.third-helix.com/2013/09/adding-to-unitys-built-in-classes-using-extension-methods/) to make your life easier.

Highlights include:

<dl>
<dt>GameObject</dt>
<dd>GetOrAddComponent(), DestroyComponent(), InstantiateChild(), ForAllComponents(), GetRendererBounds(), GetColliderBounds(), SetLayerRecursively()</dd>

<dt>Vector2</dt>
<dd>WithX(), WithY(), ToVector3()</dd>

<dt>Vector3</dt>
<dd>WithX(), WithY(), WithZ(), WithScale(), WithLength(), ToVector2()</dd>

<dt>Transform</dt>
<dd>AttachChildren(), MultiplyScale(), ClearLocalPosition(), GetChildren()</dd>

<dt>Boundsx (static methods)</dt>
<dd>EncapsulateAll()</dd>

<dt>Randomx (static methods)</dt>
<dd>InBounds(), OnLine(), AbsRange(), CoinToss()</dd>

</dl>
