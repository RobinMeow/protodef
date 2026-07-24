using System.Diagnostics;

public static class Assert
{
    [Conditional("DEBUG")]
    public static void NotNull(object? obj, string name)
    {
        // TODO: I think we can use reflection to get the member name of this obj
        Debug.Assert(obj != null, $"{name} may not be null.");
    }
}
