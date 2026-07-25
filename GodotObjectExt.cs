using Godot;

public static class GodotObjectExt
{
    extension(GodotObject? obj)
    {
        // TODO: thought I could use IsInstanceInvalid without specifying the static class name.
        // this extension failed somehow.
        public static bool IsInstanceInvalid(GodotObject? Obj) =>
            !GodotObject.IsInstanceValid(Obj);
        // public bool IsInstanceValidAndActive() => GodotObject.IsInstanceValid(obj) && !obj.IsQueuedForDeletion();
    }

    // extension(Node3D node)
    // {
    //     public bool IsInstanceInvalid(GodotObject? _obj) => !GodotObject.IsInstanceValid(_obj);
    // }
}
