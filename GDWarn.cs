using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Godot;

public static class GDWarn
{
    [Conditional("DEBUG")]
    public static void HasScript<T>(
        PackedScene packedScene,
        Action<string> addWarn,
        [CallerArgumentExpression(nameof(packedScene))] string arg = ""
    )
        where T : Node
    {
        if (packedScene == null)
        {
            addWarn($"{arg} is not set.");
            return;
        }

        SceneState state = packedScene.GetState();
        if (state.GetNodeCount() == 0)
        {
            addWarn($"Exported member '{arg}' is empty or corrupted (has no root node).");
            return;
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
                    return;
                }
            }
        }

        addWarn(
            $"Expected exported member '{arg}' to have '{expectedName}.cs' attached to its root."
        );
    }
}
