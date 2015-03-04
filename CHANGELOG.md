Changelog for ChicoPlugins
====

v0.4 (March 3, 2015)
----

* New: "Movers" module. Quick components to make objects spin, bounce, or face targets.
* New: "MouseManager" class. Helpful for tracking simple mouse and touch input over multiple frames.
* Changed: ComponentPool names are now read directly from scene hierarchy.
* Removed: CUI module.
* Improved documentation for several classes.


v0.3 (October 15, 2014)
----

* New: ComponentPools now provide static functions TryGetNext, TryGetNextAt, TryAdd (my favorite new feature).
* New: AudioPool (a ComponentPool for AudioSource components).
* New: AddComponentMenu entries for ChicoPlugins components.
* New: Vector3 extension methods WithScaleX, WithScaleY, WithScaleZ, WithClampX, WithClampY, WithClampZ.
* New: CUITextWidget (similar to CUITextButton, minus actually being a button).
* New: CUIText adds GetMessage, GetColor methods.
* Changed: ComponentPool settings are now serialized via simple public fields.
* Changed: TweenCam replaces LerpState with separate InterpParam, InterpState methods.
* Changed: CUIController now caches "last frame" values in LateUpdate(), so that they're easier to use from other scripts.
* Fixed: MusicPlayer now correctly handles music sources with base volume lower than 1.
* Fixed: CUIController on mobile was reporting last-touched position when no touch present (due to change in Unity API).


v0.2 (February 20, 2014)
----

* License: ChicoPlugins is now available under the MIT license.
* New: "TweenCam" module, a simple camera (specify location, view target).
* New: "Loading" module, better transitions for loading levels.
* New: "UI" module, work-in-progress toward a bare-bones UI library.
* New: "Audio" module, work-in-progress toward music control scripts.
* Fixed: Randomx.OnLine test was erroring out intermittently.


v0.1.1 (January 4, 2014)
----

* Documentation improvements.
* Basic testing via UnTest: https://github.com/tenpn/untest
* Fixed: ShuffleBag now correctly removes items as they are used.
* Fixed: Vector3.WithY() was returning bogus vectors.
* Feature: Vector3.Approx(), currently used for internal testing.


v0.1 (January 2, 2014)
----

* Initial release.
