#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.Core.Target
nuget AltCover.Fake //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open AltCover_Fake.DotNet.Testing

Target.create "install" (fun _ ->
    Trace.log " --- Installing dependencies --- "
    DotNet.exec id "paket" "install" |> ignore
    DotNet.exec id "paket" "restore" |> ignore)

Target.create "build" (fun _ ->
    Trace.log " --- Building files --- "
    DotNet.build id "")

Target.create "test" (fun _ ->
    Trace.log " --- Running Tests ---"
    let setBaseOptions (o: DotNet.Options) =
        { o with
              CustomParams =
                  Some "/p:AltCover=true /p:AltCoverAssemblyFilter=\"ConsoleApplication|FSharp.Core|Tests|xunit\""
              WorkingDirectory = Path.getFullName "./Tests"
              Verbosity = Some DotNet.Verbosity.Minimal }

    DotNet.test (fun o -> o.WithCommon(setBaseOptions)) "Tests.fsproj")

Target.create "coverage" (fun _ ->
    Trace.log " --- Generating Code Coverage ---"
    Shell.Exec("reportgenerator", "-reports:coverage.xml -targetdir:coverage", "./Tests") |> ignore)

Target.create "setup" (fun _ -> Trace.log " --- Project is ready --- ")

open Fake.Core.TargetOperators

// *** Define Dependencies ***
"install" ==> "build" ==> "test" ==> "setup"

Target.runOrDefault "setup"
