// Copyright (c) William Kent. All rights reserved.

namespace Sunburst.Reunion2.CppWinRT.BuildTasks;

/// <summary>
/// Contains utility extension methods.
/// </summary>
internal static class ExtensionMethods
{
    /// <summary>
    /// Appends a command-line switch that has no separate value, without any quoting,
    /// if the <paramref name="flag"/> is <see langword="true"/>. This method appends
    /// a space to the command line (if it's not currently empty) before the switch.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="CommandLineBuilder"/> to modify.
    /// </param>
    /// <param name="switch">
    /// The switch to append to the command line.
    /// </param>
    /// <param name="flag">
    /// Whether or not the switch should be appended.
    /// </param>
    internal static void AppendSwitchIfTrue(this CommandLineBuilder builder, string @switch, bool flag)
    {
        if (flag) builder.AppendSwitch(@switch);
    }
}
