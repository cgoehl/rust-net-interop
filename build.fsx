#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.Core.Target //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake
open Fake.IO
open Fake.IO.Globbing
open Fake.IO.Globbing.Operators
open Fake.Core
open System.IO

// Properties

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

Target.create"NativeBackend" (fun _ -> 
  run cargo ["build"] backendDir
)

Target.create "FSharpClient" (fun _ -> 
  run dotnet ["run"] clientDir
)

Target.create "CopyNativeLib" (fun _ ->
  Trace.trace "Inside CopyNativeLib"
  let clientBinDir =Path.combine clientDir "bin/Debug/net6.0"
  !!(Path.combine backendDir  "target/debug/librust_backend.*")
    |> Shell.copy clientBinDir
)

open Fake.Core.TargetOperators
"NativeBackend"
  ==> "CopyNativeLib"
  ==> "FSharpClient"

// start build
Target.runOrDefault "FSharpClient"