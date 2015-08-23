#r "./src/packages/FAKE.4.1.4/tools/FakeLib.dll"

open Fake
open System.IO;

//RestorePackages()
 
// Properties
let buildDir = "./build/"
let deployDir = "./deploy/"
 
// version info
let version = environVarOrDefault "PackageVersion" "1.0.0.0-beta"  // or retrieve from CI server
let summary = "Open source, portable library from empowering reactive programming with Caliburn Micro"
let copyright = "Ian Bebbington, 2014"
let tags = "Caliburn Micro Reactive Rx Observable"
let description = "Open source, portable library from empowering reactive programming with Caliburn Micro"

let portableAssemblies = [ "Caliburn.Micro.Reactive.Extensions.dll"; "Caliburn.Micro.Reactive.Extensions.pdb"; ]

let libDir = "lib"
let portableTarget = "portable-win81+wpa81+net45+uap10.0"
let uapTarget = "uap"
 
// Targets
Target "Clean" (fun _ ->
    CleanDirs [ deployDir ]
)
 
Target "Build" (fun _ ->
   !! "./src/**/*.csproj"
     |> MSBuildRelease buildDir "Build"
     |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
    !! (buildDir + "*.Test.dll") 
    ++ (buildDir + "*.Tests.dll")
      |> NUnit (fun p ->
          {p with
             DisableShadowCopy = true;
             OutputFile = buildDir + "TestResults.xml" })
)

Target "Package" (fun _ ->

    portableAssemblies |> List.map(fun a -> buildDir @@ a) |> CopyFiles deployDir  

    let portableFiles = portableAssemblies |> List.map(fun a -> (a, Some(Path.Combine(libDir, portableTarget)), None))
    let uapFiles = portableAssemblies |> List.map(fun a -> (a, Some(Path.Combine(libDir, uapTarget)), None))

    let dependencies = getDependencies "./src/Caliburn.Micro.Reactive.Extensions/packages.config" |> List.filter (fun (name, version) -> name <> "FAKE")

    printfn "%A" dependencies

    NuGet (fun p -> 
        {p with
            Authors = [ "Ian Bebbington" ]
            Project = "Caliburn.Micro.Reactive.Extensions"
            Description = description
            Summary = summary
            Copyright = copyright
            Tags = tags
            OutputPath = deployDir
            WorkingDir = deployDir
            Version = version
            Dependencies = dependencies
            Files = portableFiles @ uapFiles
            Publish = false }) 
            "./Caliburn.Micro.Reactive.Extensions.nuspec"
)

Target "Run" (fun _ -> 
    trace "FAKE build complete"
)
  
// Dependencies
"Clean"
  ==> "Build"
//  ==> "Test"
  ==> "Package"
  ==> "Run"
 
// start build
RunTargetOrDefault "Run"