#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.Core.Target //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO

Target.create "install" (fun _ ->
  Trace.log " --- Installing dependencies --- "
  Shell.Exec ("paket", "install") |> ignore
  Shell.Exec ("paket", "restore") |> ignore
)

Target.create "build" (fun _ ->
  Trace.log " --- Building files --- "
  Shell.Exec ("dotnet", "build") |> ignore
)

Target.create "test" (fun _ -> 
  Trace.log " --- Running Tests ---"
  Shell.Exec ("dotnet", "test") |> ignore
)

Target.create "setup" (fun _ -> Trace.log " --- Project is ready --- ")

open Fake.Core.TargetOperators

// *** Define Dependencies ***
"install"
  ==> "build"
  ==> "test"
  ==> "setup"

Target.runOrDefault "setup"
