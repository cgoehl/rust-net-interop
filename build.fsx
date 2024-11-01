#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.IO
open Fake.Core

// Properties
let buildDir = "./build/"
let backendDir = Path.getFullName "rust_backend"
let clientDir = Path.getFullName "fsharp_client"

let createProcess exe args dir =
    // Use `fromRawCommand` rather than `fromRawCommandLine`, as its behaviour is less likely to be misunderstood.
    // See https://github.com/SAFE-Stack/SAFE-template/issues/551.
    CreateProcess.fromRawCommand exe args
    |> CreateProcess.withWorkingDirectory dir
    |> CreateProcess.ensureExitCode

let dotnet args dir = createProcess "dotnet" args dir
let cargo args dir = createProcess "cargo" args dir
let run proc arg dir = proc arg dir |> Proc.run |> ignore

Target.create "Default" (fun _ ->
  run cargo ["build"] backendDir
  run dotnet ["run"] clientDir
)

// start build
Target.runOrDefault "Default"