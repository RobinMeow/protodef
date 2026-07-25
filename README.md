# protodef

## TODOs

- pre-defined level (fixed locations, where one can place towers)
- top down view (side-angled) (preferably fixed view)
- predefined unit pathing (the one way)
- units despawn, when they reached the end
- health system (how many unit one may miss)
- tower attack/focus system onto the units
- tower placement
- weapon scene (it has `weapon-ballista` (rotatabe) which needs to be ontop of `weapon-turret` and spawn location for arrows as child of the `weapon-ballista`)
- per kill counter (points) which can be traded into towers
- start with enough points for free to make one tower

- sound effect
- background music
- hud visuals (tower level)

## Style Guide

csharp: PascalCase
Nodes: PascalCase
assets (files and directories): snake_case
thrid party assets: //addons/

## Assets

output after importing assets
```sh
  ERROR: servers/rendering/renderer_rd/storage_rd/mesh_storage.cpp:866 - Parameter "mesh" is null.
res://assets/kenney_tower-defense-kit/Models/FBX format/Textures/colormap.png: Texture detected as used in 3D. Enabling mipmap generation and setting the texture compression mode to VRAM Compressed (S3TC/ETC/BPTC).
res://assets/kenney_tower-defense-kit/Models/GLB format/Textures/colormap.png: Texture detected as used in 3D. Enabling mipmap generation and setting the texture compression mode to VRAM Compressed (S3TC/ETC/BPTC).
res://assets/kenney_tower-defense-kit/Models/OBJ format/Textures/colormap.png: Texture detected as used in 3D. Enabling mipmap generation and setting the texture compression mode to VRAM Compressed (S3TC/ETC/BPTC).
```

## Keymaps

hold ctrl + mb3 to have fine zoom in 3d view
hold mb3 to orbit
hold shift + mb3 to pan
press 7 for a top down view orthogonal


## Low Prio

- arrows should fly at a straight line. For that it would need to request the future target dest from the target enemy (which in turn optionally could be extended with out of range behavior) to fire the arrow in a straight line. For now I'm just gonna increase the speed to make it look more straigt
