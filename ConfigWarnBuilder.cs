using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Godot;

public sealed class ConfigWarnBuilder
{
    readonly List<string> warnings = new();

    public ConfigWarnBuilder() { }

    public ConfigWarnBuilder NotNull(
        object? obj,
        [CallerArgumentExpression(nameof(obj))] string? msg = null
    )
    {
        if (obj == null)
            warnings.Add($"{msg} is not set.");

        return this;
    }

    public ConfigWarnBuilder That(
        bool condition,
        [CallerArgumentExpression(nameof(condition))] string? msg = null
    )
    {
        if (condition == false)
            warnings.Add($"{msg}. Expected to be truthy.");

        return this;
    }

    public string[] Build() => warnings.ToArray();
}
