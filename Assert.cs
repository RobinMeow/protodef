using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

public static class Assert
{
    [Conditional("DEBUG")]
    public static void NotNull(object? obj, string name)
    {
        // TODO: I think we can use reflection to get the member name of this obj
        Debug.Assert(obj != null, $"{name} may not be null.");
    }

    [Conditional("DEBUG")]
    public static void True([DoesNotReturnIf(false)] bool condition, string name)
    {
        Debug.Assert(condition, name);
    }

    [Conditional("DEBUG")]
    public static void False(
        [DoesNotReturnIf(false)] bool condition,
        [CallerArgumentExpression("condition")] string? msg = null
    )
    {
        Debug.Assert(condition, msg);
    }
}
