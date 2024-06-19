# Fish

A (bad but functional) Godot simulation of fish scooting around a spherical planet.

One fish is special and we have a camera view for that. Another 50 fish are spawned randomly around the planet and given random directions and direction timers so there is a little randomness.

The core 'rotation' code was adapted from this post: https://forum.godotengine.org/t/how-to-rotate-an-object-around-a-point/62631/6

Some nice to haves:
* The ability to rotate the camera around the planet (which would look like rotating the simulation by dragging the planet)
* Control the camera fish with WASD or arrow movement keys
* Control the camera fish with up/down (jump/crouch) movement keys
* Smoothly rotate the fish to their new direction rather than snapping
  * I tried a LERP but it didn't quite work how I wanted

It uses physics so sometimes fish can bump and stick on each other, but they usually clear up when they change direction.

Thanks!