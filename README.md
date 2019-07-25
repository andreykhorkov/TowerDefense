original task:

Implement a Tower Defense turret combat simulation using simple primitives in a Unity3D game
scene. Ensure the game is architected following sound Unity3D game development principles,
is extensible, and accounts for building out the game’s framework cleanly in the future.

The Game:
  * When played, a camera will display 3D bird’s eye or isometric view of a single game
level.
  * There is 1 tower defense “Turret” asset next to the path near the center of the game
level. It has the following attributes:
      - It will fire on units within a 360 degree radius of itself.
      - The targeting area distance should be definable (Default: No larger than 1/20th of
the game play area.
      - It will fire a single projectile at a target enemy who is the furthest along the path
and within the targeting area.
      - The turret will fire at a definable rate (Default: every 500ms)
      - An enemy hit by the projectile will take a definable amount of damage (Default:
50). Enemies with no health remaining are destroyed.
      - Projectiles always hit their target
  * Enemies will spawn and travel down a path past the turret. Enemies need to have a
definable amount of health (Default: 100). If an enemy survives past the turret, it may be
destroyed when it reaches the edge of the game level.
Extra Credit:
  * Every 10 seconds, increase the frequency of enemies spawning and their movement
speed.
  * Add in a simple UI scoring mechanism, and a timer.
  * Create two different enemy types, one that moves slow and takes multiple hits to
destroy, and one that moves twice as fast that only needs to be hit once. Randomize the
enemy type that spawns each wave.
  * Have fun! Add your own unique twist, but do not go over the time limit.


Hi.
Even though i made it in classic way it's probably something that should be solved with Data Oriented Design 
(entity component system). Which is i need to do more research in. Btw.. here we also have a candidate for work on another thread. That 
enemy choosing could be a background task of course with some modifications, cuz we can't use Unity's API there.

And i don't quite understood a request of making targeting area no larger that 1/20th of game play area.. My turret about 1/20th part itself :) So there is some other adjustable values.

The version of Unity i used is 2019.0.14
