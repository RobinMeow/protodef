using Godot;

public static class DictionaryExtensions
{
    extension(Godot.Collections.Dictionary dictionary)
    {
        public IntersectRayResult ToResult()
        {
            return new IntersectRayResult(dictionary);
        }
    }
}
