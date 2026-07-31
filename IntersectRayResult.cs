using Godot;

// NOTE: using class instead of readonly struct, because dotnets .Value and .HasValue design is terrible
// alternatively you could use a design which lazy loads using methods PointOfIntersection() to read the object only when needed
// isntead of casting them all, without all in use.
// could also extend .AsResult() with args to tell which one to eager load .AsResult("Collider", "PointOfIntersection")
public class IntersectRayResult
{
    public IntersectRayResult(Godot.Collections.Dictionary dict)
    {
        Assert.True(HasIntersected(dict));
        PointOfIntersection = (Vector3)dict["position"];
        Collider = dict["collider"].AsGodotObject();
    }

    public readonly Vector3 PointOfIntersection;

    public readonly GodotObject Collider;

    // NOTE: implement other dict fields as needed
    public static bool HasIntersected(Godot.Collections.Dictionary dict) => dict.Count > 0;
}
