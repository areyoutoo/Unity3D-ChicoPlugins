ChicoPlugins
============

Suite of helpful Unity3D code files.

This project is a collection of files and modules that I've found useful for multiple projects.


Installation
----

This is fundamentally a loose collection of scripts. Some of them are useful individually, so feel free to just download one or two as long as you keep to the MIT license.

To import the whole set in one go, you can download a `.unitypackage` file from the releases link.

If you'd rather get a development copy, you can clone the repo and open the `Project` folder in Unity:

* `git clone https://github.com/areyoutoo/ChicoPlugins.git`
* In Unity: `File` > `New Project` > `Browse...` > Find the cloned repo and select the "Project" folder
* Unity will automatically create metadata files (asset database, project settings, solutions, etc.)


Bags module
----

The Bags module has some helper classes to collect values and retrieve them at random.

* **RandomBag** returns items at random, like a fair dice roll.
* **ShuffleBag** returns items at random, like a deck of cards. It also refills automatically once empty.
* **WeightedBag** returns items with weighted randomness, like a biased dice roll.

[Managed randomness](http://gamedevelopment.tutsplus.com/tutorials/shuffle-bags-making-random-feel-more-random--gamedev-1249) sometimes [feels more authentic](http://seanmonstar.com/post/708989796/a-less-random-generator) for gameplay.

You can retrieve a value from a bag by calling its `GetNext()` method. Most of the bags can be filled by calling `Add(T)`, or by passing an IEnumerable to their constructor.


ComponentPools module
----

Instantiate() and Destroy() can be expensive calls; it is often more performant to recycle GameObjects using an [object pool](http://answers.unity3d.com/questions/196413/gameobject-pool-to-avoid-performance-issues-in-ins.html) pattern.

Our pools are managed directly in the scene hierarchy.

Here's an example using a ParticlePool, which stores ParticleSystem components:

    Scene                            (your scene file)
    
        ParticlePools                (empty container)
        
	        ExplosionParticlePool    (has a ParticlePool attached)
	            Explosion            (has a ParticleSystem attached)
				Explosion            (has a ParticleSystem attached)
				
            SmokeParticlePool        (has a ParticlePool attached)
	            Smoke                (has a ParticleSystem attached)
	            Smoke                (has a ParticleSystem attached)

When your code needs to play an explosion:

    ParticleSystem ps;
    ParticlePool.TryGetNextAt("ExplosionParticlePool", transform.position out ps);

That will find a copy of the "Explosion" effect and move it to our position. With default settings, it will even activate it for us, and return it to the pool once it's finished playing.

Here's a slightly more robust example:

	ParticleSystem mySmoke, myExplosion;
    ParticleSystem ps;
    if (ParticlePool.TryGetNextAt("ExplosionParticlePool", transform.position, out ps)) 
    {
	    myExplosion = ps;
	    Debug.Log("Playing Explosion", this);
    }
    else
    {
	    Debug.Log("Failed to get Explosion copy", this);
    }
	
	if (ParticlePool.TryGetNextAt("SmokeParticlePool", transform.position, out ps))
    {
	    mySmoke = ps;
	    Debug.Log("Playing Smoke", this);
    }
    else
    {
	    Debug.Log("Failed to get Smoke copy", this);
    }

If the pool is empty, default settings will clone another copy of the effect.

Repeated cloning can be attempted in batches of increasing size, with each batch occurring over multiple frames, to reduce performance overhead.

While the pool manages only a single component type, each component is attached to a GameObject, so moving a new pool member will also move anything else that's attached to it. This is something to be careful about, but can also be very useful in some circumstances.

Several built-in pool types are available:

* **ParticlePool** for ParticleSystem components.
* **AudioPool** for AudioSource components.
* **TransformPool** for Transform components (useful for just about anything).

It's also easy to build your own pool types with just a few lines of code. Check out the existing pools to see just how simple those files are.

It's easy to manage other components, including your own custom components, by creating a new class inheriting from `ComponentPool<T>`.


Miscellaneous modules
----

### MouseManager

MouseManager is a standalone class which attempts to manage mouse clicking *and* touch input together. It won't catch every elaborate case, but it usually does the trick for simple arrangements involving pointing, clicking, dragging, and so on.

This object assumes that your scene has at least two cameras: one main world camera, one UI camera. You can have others.

The class keeps track of information over multiple frames, including information about whether the mouse position hits anything in the world or UI space, what was being pointed at the last time the mouse was clicked, and so on.

As far as this class is concerned, clicking the left mouse button begins touching the screen, and releasing the button ends the touch. Multiple touches are not processed or handled.


### Loading module

Simple loading screen support, similar to [this blog post](http://chicounity3d.wordpress.com/2014/01/25/loading-screen-tutorial/).

First, create a level that *is* a splash screen. Second, show that level between scenes. This module makes it easy and quick to get rolling with a basic setup.

	//loads the next scene, using default scene "Loading" as splash screen
    LevelLoader.Load("Menu");

	//loads the next scene, using specified scene as splash screen
    LevelLoader.Load("Menu", "LoadingScene");

You can build any loading screen that you want. Or, you can use an empty scene which contains only the built-in `SimpleLoadUI` component; that script will show a simple text message using OnGUI calls.


### TweenCam module

TweenCam tracks its state with two points in space: where is the camera, and what is it looking at? You can provide your own functions to provide that state, once per frame, and cleanly tween between output from two such functions.

For advanced use, TweenCam also allows you to control the camera's FOV/size and "up" direction.


### Audio module

The **MusicPlayer** script can be used to quickly convert an AudioSource to a music source (ignores audio effects, falloff distance, listener volume, etc.)

The **MusicLooper** script does the same, but also loops the audio. If you call `DontDestroyOnLoad` for this object, it could be used to play music throughout your entire game.


### Movers module

The movers module contains some very simple movement scripts. Attach one or more scripts to a GameObject to cause movement once per frame:

* **Spinner** rotates the object once per frame.
* **Bouncer** makes the object bounce up and down over time.
* **Facer** makes the object look at another object each frame.

The movers can be customized, for example by setting a new axis of rotation, using local or world space, changing the speed or magnitude of movement, and so on.


### Extensions module

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
