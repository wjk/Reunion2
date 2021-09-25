// Copyright (c) William Kent. All rights reserved.

namespace Sunburst.Reunion2.CppWinRT.BuildTasks;

#pragma warning disable SA1600

internal class CppWinRT : ToolTask
{
    [Required]
    public ITaskItem? ToolLocation { get; set; } = null;

    [Required]
    public ITaskItem? OutputDirectory { get; set; } = null;

    [Required]
    public ITaskItem[] InputFiles { get; set; } = Array.Empty<ITaskItem>();

    public ITaskItem[] ReferenceFiles { get; set; } = Array.Empty<ITaskItem>();

    public ITaskItem? PrecompiledHeader { get; set; } = null;

    public ITaskItem? ComponentOutputDirectory { get; set; } = null;

    public string RootNamespace { get; set; } = string.Empty;

    public bool UsePrefixes { get; set; } = false;

    public bool Optimize { get; set; } = false;

    public bool UseFastAbi { get; set; } = false;

    public bool Verbose { get; set; } = false;

    protected override string ToolName => "cppwinrt.exe";

    protected override string GenerateFullPathToTool() => ToolLocation!.GetMetadata("FullPath");

    protected override string GenerateResponseFileCommands()
    {
        CommandLineBuilder builder = new();

        builder.AppendSwitch("-overwrite");
        builder.AppendSwitchIfTrue("-verbose", Verbose);
        builder.AppendSwitchIfTrue("-fastabi", UseFastAbi);
        builder.AppendSwitchIfTrue("-opt", Optimize);

        builder.AppendSwitchIfNotNull("-name ", RootNamespace);
        builder.AppendSwitchIfNotNull("-pch ", PrecompiledHeader);

        foreach (ITaskItem input in InputFiles) builder.AppendSwitchIfNotNull("-in ", input);
        foreach (ITaskItem input in ReferenceFiles) builder.AppendSwitchIfNotNull("-ref ", input);
        builder.AppendSwitchIfNotNull("-out ", OutputDirectory!.GetMetadata("FullPath").TrimEnd('\\'));

        return builder.ToString();
    }

    protected override bool SkipTaskExecution()
    {
        if (InputFiles.Length == 0) return true;
        else return false;
    }
}
