﻿#r "packages/FAKE.3.2.0/tools/FakeLib.dll"
#load "helpers.fsx"

open Fake
open Helpers
open Fake.NuGetHelper
open Fake.AssemblyInfoFile
open System

let Version = getBuildParamOrDefault "Version" "1.0.0.0-dev"

Target "Default" DoNothing

Target "Clean" (fun _ ->
    msbuild ["Clean"] []
    CleanDir NugetOutputDirectory
)

Target "GenerateVersionFile" (fun _ ->
    CreateCSharpAssemblyInfo (currentDirectory @@ "SolutionVersionInfo.cs")
        [
            Attribute.Version Version
            Attribute.FileVersion Version
            Attribute.Metadata("GitHash", Git.Information.getCurrentSHA1(currentDirectory))
        ]
)

Target "Build" (fun _ ->
    msbuild ["Build"] []    
)

Target "PackNuGet" (fun _ ->
    for nuspec in !! ("**/Trigger.NET*.nuspec") do
        trace ("Packing " + System.IO.Path.GetFileNameWithoutExtension(nuspec))
        pack nuspec
)

"Clean"
==> "GenerateVersionFile"
==> "Build"
==> "PackNuGet"
==> "Default"

RunTargetOrDefault "Default"
