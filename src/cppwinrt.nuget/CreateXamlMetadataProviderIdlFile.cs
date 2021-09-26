﻿// Copyright (c) William Kent. All rights reserved.

namespace Sunburst.Reunion2.CppWinRT.BuildTasks;

using Task = Microsoft.Build.Utilities.Task;

#pragma warning disable SA1600

public sealed class CreateXamlMetadataProviderIdlFile : Task
{
    [Required]
    public string? OutputFile { get; set; }

    [Required]
    public string? RootNamespace { get; set; }

    [Required]
    public string? XamlNamespace { get; set; }

    public bool WriteOnlyWhenDifferent { get; set; } = false;

    public bool ProvideFullXamlMetadataProviderAttribute { get; set; } = false;

    public bool DisableReferences { get; set; } = false;

    /// <inheritdoc/>
    public override bool Execute()
    {
        if (OutputFile is null)
        {
            Log.LogError("No OutputFile provided");
            return false;
        }

        if (RootNamespace is null)
        {
            Log.LogError("No RootNamesapce provided");
            return false;
        }

        if (XamlNamespace is null)
        {
            Log.LogError("No XamlNamespace provided");
            return false;
        }

        StringBuilder output = new();
        output.AppendLine("// This file is generated by the build to support Xaml apps.");
        output.AppendLine();

        if (DisableReferences)
        {
            output.AppendLine($"import \"{XamlNamespace}.Markup.idl\";");
            output.AppendLine();
        }

        output.AppendLine($"namespace {RootNamespace}");
        output.AppendLine("{");

        if (ProvideFullXamlMetadataProviderAttribute)
        {
            output.AppendLine($"    [{XamlNamespace}.Markup.FullXamlMetadataProvider]");
        }

        output.AppendLine($"    runtimeclass XamlMetaDataProvider : [default] {XamlNamespace}.Markup.IXamlMetadataProvider");
        output.AppendLine("    {");
        output.AppendLine("        XamlMetaDataProvider();");
        output.AppendLine("    }");
        output.AppendLine("}");

        string newContents = output.ToString();

        if (WriteOnlyWhenDifferent)
        {
            if (File.Exists(OutputFile))
            {
                string oldContents = File.ReadAllText(OutputFile);
                if (oldContents.Length == newContents.Length)
                {
                    if (oldContents == newContents)
                    {
                        Log.LogMessage(MessageImportance.Low, "Not overwriting unchanged file {0}", OutputFile);
                        return true;
                    }
                }
            }
        }

        File.WriteAllText(OutputFile, newContents);
        return true;
    }
}
