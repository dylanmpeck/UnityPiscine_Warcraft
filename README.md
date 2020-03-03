# UnityPiscine_Warcraft
![alt-text](https://github.com/dylanmpeck/UnityPiscine_Warcraft/blob/master/UPWarcraft.gif)

[Play Game](https://dylanmpeck.itch.io/unity-piscine-warcraft?secret=roGFlR6oLYU6YjGGBE3bySxS0)
___
## Overview
UPWarcraft is an RTS exercise done in two days for a Unity bootcamp. In UPWarcraft, there are two factions (human and orc) and each have a base of 5 buildings. Every 30 seconds each base spawns a new unit to fight the other, however each building a faction loses will increase that spawn time. The first team to destroy the other's base wins the game.

The goal of the exercise was to implement basic 2D RTS mechanics and work more with Unity's animation system, Mecanim. Each unit is selectable by mouse click and can move, attack, and be damaged/die. With the exception of the death animation, each animation is animated in eight different directions and the proper animation plays depending on which way he's facing. There is also a very simple AI implemented that moves towards the player base and attacks the first thing it sees with preference toward units, but will pull back to its own base if it's being attacked.

On top of the basic requirements, I implemented my own BFS pathfinding algorithm to make sure units didn't walk through buildings and over water and mountain tiles. To do this, I make a 2d bool array the size of my tile map where true means the tile is walkable and false means the tile isn't. Then, whenever I move a unit, I breadth first search through this graph and take the first path that gets me to the destination node/tile. It's a little clunky right now as the tiles for the map which I'm using for my BFS graph are a bit large and units will still walk into each other, but it worked great for an exercise I needed to complete under the clock. Also, something like A* or even Dijkstra's would have gotten smoother results - with BFS sometimes weird-looking pathing is evaluated as the same distance as a more straightforward path.

Lastly, I'm playing audio sounds everytime a footman is selected, moves, attacks, or dies. In order to get smooth sound and prevent sounds from clipping off everytime an action plays, I flip flop between two general audio sources and crossfade the old sound into the new sound.
