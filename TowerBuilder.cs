using System;
using System.Collections.Generic;
using Godot;

// INFO: cell == tile
// cell is the term used by GridMap

[Tool]
public partial class TowerBuilder : Node3D
{
    [Export]
    GridMap gridMap = null!;

    [Export]
    Node3D hoverMesh = null!;

    [Export]
    string[] buildableTileNames = Array.Empty<string>();

    [Export]
    PackedScene tower = null!;

    readonly HashSet<Vector3I> occupiedCells = new();
    Camera3D camera = null!;
    World3D world = null!;

    public override void _Ready()
    {
        if (Engine.IsEditorHint())
            return;

        world = GetWorld3D();
        Assert.NotNull(world);

        camera = GetViewport().GetCamera3D();
        Assert.NotNull(camera);

        hoverMesh.Visible = false;
    }

    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
            return;

        bool show_hover =
            TryGetHoveredCell(out Vector3I cellCoord)
            && IsBuildableCell(cellCoord)
            && !IsOccupiedCell(cellCoord);

        if (show_hover)
            ShowTileHover(cellCoord);
        else
            HideTileHover();
    }

    /// <summary>currently hovered or clicked cell</summary>
    bool TryGetHoveredCell(out Vector3I cellCoord)
    {
        Vector2 mouse_pos = GetViewport().GetMousePosition();
        const float ray_length = 1000.0f;

        Vector3 origin = camera.ProjectRayOrigin(mouse_pos);
        Vector3 dest = origin + camera.ProjectRayNormal(mouse_pos) * ray_length;

        var query = PhysicsRayQueryParameters3D.Create(origin, dest);

        var res = world.DirectSpaceState.IntersectRay(query).AsResult();

        if (res == null || res.Collider != gridMap)
        {
            cellCoord = default;
            return false;
        }

        Vector3 global_pos = res.PointOfIntersection;
        cellCoord = gridMap.LocalToMap(gridMap.ToLocal(global_pos));
        return true;
    }

    bool IsOccupiedCell(Vector3I cellCoord)
    {
        return occupiedCells.Contains(cellCoord);
    }

    bool IsBuildableCell(Vector3I cellCoord)
    {
        int tile_idx = gridMap.GetCellItem(cellCoord);
        if (tile_idx == GridMap.InvalidCellItem)
            return false;

        string hovered_tile_name = gridMap.MeshLibrary.GetItemName(tile_idx).ToLower();

        foreach (string buildable_tile_name in buildableTileNames)
            if (hovered_tile_name == buildable_tile_name)
                return true;

        return false;
    }

    void ShowTileHover(Vector3I cell_coords)
    {
        Vector3 cell_local = gridMap.MapToLocal(cell_coords);
        // center hover mesh to the tile
        hoverMesh.GlobalPosition = gridMap.ToGlobal(cell_local);
        hoverMesh.Visible = true;
    }

    void HideTileHover()
    {
        hoverMesh.Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (!@event.IsPressed(MouseButton.Left))
            return;
        if (!TryGetHoveredCell(out Vector3I cellCoord))
            return;
        if (!IsBuildableCell(cellCoord))
            return;
        if (IsOccupiedCell(cellCoord))
            return;

        PlaceTower(cellCoord);
    }

    void PlaceTower(Vector3I cellCoord)
    {
        Node3D _tower = tower.Instantiate<Node3D>();
        GetTree().Root.AddChild(_tower); // TODO: make a node as TowerCollection? otherwise they could pollute the scene view. Not that it matter probably..
        _tower.GlobalPosition = gridMap.ToGlobal(gridMap.MapToLocal(cellCoord));
        occupiedCells.Add(cellCoord);
    }

    public override string[] _GetConfigurationWarnings()
    {
        return new ConfigWarnBuilder()
            .NotNull(gridMap)
            .NotNull(gridMap?.MeshLibrary)
            .NotNull(hoverMesh)
            .That(buildableTileNames.Length > 0)
            .NotNull(tower)
            .Build();
    }
}
