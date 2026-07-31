using Godot;

public static class DictionaryExtensions
{
    extension(Godot.Collections.Dictionary dictionary)
    {
        public IntersectRayResult? AsResult()
        {
            return IntersectRayResult.HasIntersected(dictionary)
                ? new IntersectRayResult(dictionary)
                : null;
        }
    }
}
