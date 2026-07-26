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

    public ConfigWarnBuilder HasScript<T>(
        PackedScene packedScene,
        [CallerArgumentExpression(nameof(packedScene))] string arg = ""
    )
        where T : Node
    {
        if (packedScene == null)
        {
            warnings.Add($"{arg} is not set.");
            return this;
        }

        SceneState state = packedScene.GetState();
        if (state.GetNodeCount() == 0)
        {
            warnings.Add($"Exported member '{arg}' is empty or corrupted (has no root node).");
            return this;
        }

        string expectedName = typeof(T).Name;

        // NOTE: don't think I need this currently for checking script only
        // if (state.GetNodeType(0) == expectedName)
        //     return;

        for (int i = 0; i < state.GetNodePropertyCount(0); i++)
        {
            if (state.GetNodePropertyName(0, i) == "script")
            {
                Script script = state.GetNodePropertyValue(0, i).As<Script>();

                if (
                    script != null
                    && script.ResourcePath.EndsWith(
                        $"{expectedName}.cs",
                        StringComparison.InvariantCulture
                    )
                )
                {
                    return this;
                }
            }
        }

        warnings.Add(
            $"Expected exported member '{arg}' to have '{expectedName}.cs' attached to its root."
        );

        return this;
    }

    public string[] Build() => warnings.ToArray();
}
