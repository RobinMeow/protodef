using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

public static class Assert
{
    [Conditional("DEBUG")]
    public static void NotNull(
        object? obj,
        [CallerArgumentExpression(nameof(obj))] string? msg = null
    )
    {
        Debug.Assert(obj != null, $"{msg} may not be null.");
    }

    [Conditional("DEBUG")]
    public static void That(
        bool condition,
        [CallerArgumentExpression(nameof(condition))] string? msg = null
    )
    {
        Debug.Assert(condition, $"{msg}. Expected to be truthy.");
    }

    [Conditional("DEBUG")]
    public static void True(
        [DoesNotReturnIf(false)] bool condition,
        [CallerArgumentExpression("condition")] string? msg = null
    )
    {
        Debug.Assert(condition, msg);
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
