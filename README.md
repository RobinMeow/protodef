# protodef

your average tower defense game.

## Keymaps

hold ctrl + mb3 to have fine zoom in 3d view
hold mb3 to orbit
hold shift + mb3 to pan
press 7 for a top down view orthogonal

## Creating MeshLibraries

create a scene (its temporary, name doesnt matter)
for interactiable tiles, you want to create a collision shape.
`make local`, so you can click on the meshinstance3d.
(Optional: you can make the mesh the root, and the origianl root can be deleted)
with that selected a `Mesh` button will become visible in the scene view's top bar.
Click -> create sibling single convex. (trimish if accuracy is required at cost of perf)
append a child staticbody3d, make the collisionshape a child of it.
save. Scene > Export as MeshLib (and now you can delete the tscn again)

## Coding Guidelines

csharp: PascalCase
Nodes: PascalCase
assets (files and directories): snake_case
thrid party assets: //addons/

- sort methods as you would like to read them from reading the file top down
- within a method with branches, for performance put the most likely use case first (this often is also the best readable) prefer readability if the order of events is not the order of most likely events
- avoid nesting (utilise guard clauses)
- prefer modular short methods (modular methods become short by nature. Modular is the term to focus on)
- keep events (onclick/ready/process) free of logic (unless its a very small file) events only delegate and orchastre work to other functions in larger files
- when `[Tool]` is used make sure to include early exit clauses in all godot lifecycle methods which contain game runtime only logic _(tool will make ready/process/exittree etc. run in the editor. e.g. for gizmos. It also enables _GetConfigurationWarnings() which is the only fn, assumingly, which does not run at game time)_

## Tasks

tasks can be split it smaller tasks and solved independently. e.g. you can implement the wave system without the hud, and leave those points open for later.

- [x] pre-defined level (fixed locations, where one can place towers)
- [x] top down view (side-angled) (preferably fixed view)
- [x] predefined unit pathing (the one way)
- [x] units despawn, when they reached the end
- [x] tower attack/focus system onto the units
- [x] tower placement
- [ ] player health system (lose a health point when an enemy reches the end)
- [ ] visuals for spawn and player base (assets are available)
- [ ] [background music](#background-music)
- [ ] [weapon](#weapon)
- [ ] [point system](#point-system)
- [ ] [wave system](#wave-system)

**Extract into smaller tasks.**
- sound effects (ballistics hit, losing a player health point, enemy kill, etc.. checkout the todos in the code, or look for existing signal for places where sounds can make sense)
- same as sound effect but partical system (blood scatters, etc..)

### point system

- [ ] start with enough points for free to make one tower
- [ ] per kill counter (points) which can be traded into towers (functional only)
- [ ] make it visual in hud

### weapon

- [x] add weapon mesh to tower and move spawn pos responsibility to weapon script
- [ ] extract the weapon logic (firing is reponsibility of the weapon which does not yet exist so the tower does)
- [ ] weapon can be made rotatable (no longer static) and using the LookAt() fn to aim at the unit which is currently has in focus
- [ ] balista-bow is seprate from the horizntal rotating base so the bow can rotate up/down and the bose horizontally

> the weapon will be a child node of the tower scene

### wave system

- [ ] pre-defined waves through code for the first basic level
- [ ] make waves configurable through the inspecter to they can be reused for new levels
- [ ] hud should show the current wave
- [ ] (maybe/optional) hud should show how many more waves will come

### background music

- [ ] background music
- [ ] hud to mute

### future idea milestones (not planned/do not work on theses)

- make more levels (I think I want to call them maps?)
- using the assets to make maps which have different elevated heights
- large enough maps, which require camera movements
- oblivion/space background (just an idea, to make the game look better, we dont have to bother using the gridmap making visual pleasing environments, if we can just make the line and buildable tiles and everything around it is "space")
- using the assets for more levels/maps
- use wood structures for incremental tower building
