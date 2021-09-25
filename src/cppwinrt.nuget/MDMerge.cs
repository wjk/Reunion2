// Copyright (c) William Kent. All rights reserved.

namespace Sunburst.Reunion2.CppWinRT.BuildTasks;

#pragma warning disable SA1600

public sealed class MDMerge : ToolTask
{
    [Required]
    public string MdMergePath { get; set; } = string.Empty;

    [Required]
    public ITaskItem? MergedDirectory { get; set; }

    [Required]
    public ITaskItem[] MetadataDirectories { get; set; } = Array.Empty<ITaskItem>();

    [Required]
    public ITaskItem[] InputFiles { get; set; } = Array.Empty<ITaskItem>();

    [Required]
    public int NamespaceMergeDepth { get; set; } = 0;

    public bool ValidateOutput { get; set; } = false;

    protected override string ToolName => "mdmerge.exe";

    protected override string GenerateFullPathToTool() => MdMergePath;

    protected override string GenerateResponseFileCommands()
    {
        CommandLineBuilder builder = new();
        builder.AppendSwitchIfTrue("-v", ValidateOutput);

        foreach (var dir in MetadataDirectories)
        {
            string fullPath = dir.GetMetadata("FullPath").TrimEnd('\\');
            builder.AppendSwitchIfNotNull("-metadata_dir ", fullPath);
        }

        foreach (var file in InputFiles)
        {
            builder.AppendSwitchIfNotNull("-i ", file);
        }

        if (NamespaceMergeDepth != 0)
        {
            builder.AppendSwitchIfNotNull("-n:", NamespaceMergeDepth.ToString());
        }

        string outputPath = MergedDirectory!.GetMetadata("FullPath").TrimEnd('\\');
        builder.AppendSwitchIfNotNull("-o ", outputPath);
        builder.AppendSwitchIfNotNull("-partial ", NamespaceMergeDepth.ToString());

        return builder.ToString();
    }

    protected override bool ValidateParameters()
    {
        if (string.IsNullOrEmpty(MdMergePath))
        {
            Log.LogError("MdMerge path parameter must be provided");
            return false;
        }

        if (MergedDirectory is null)
        {
            Log.LogError("MergedDirectory path must be provided");
            return false;
        }

        return true;
    }
}
