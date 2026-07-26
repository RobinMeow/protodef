using Godot;

public class IntersectRayResult(Godot.Collections.Dictionary dict)
{
    bool has_intersected_was_called = false;
    bool has_intersected = false;

    /// <summary>
    /// Whether or not the ray intersected with something.
    /// Call this before accessing any of the other methods,
    /// otherwise an Expection(has_intersected_was_called) is raised (Only in DEBUG).
    /// </summary>
    public bool HasIntersected()
    {
        has_intersected_was_called = true;
        has_intersected = dict.Count > 0;
        return has_intersected;
    }

    public GodotObject CollidingObj()
    {
        Assert.That(has_intersected_was_called);
        Assert.True(
            has_intersected,
            $"Don't access {nameof(CollidingObj)} when {nameof(HasIntersected)} returned false."
        );
        return dict["collider"].AsGodotObject();
    }

    public Vector3 PointOfIntersection()
    {
        Assert.That(has_intersected_was_called);
        Assert.True(
            has_intersected,
            $"Don't access {nameof(PointOfIntersection)} when {nameof(HasIntersected)} returned false."
        );
        return (Vector3)dict["position"];
    }

    // NOTE: implement other dict fields as needed
}
