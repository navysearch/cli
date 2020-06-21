// Learn more about F# at http://fsharp.org

open System
open dotenv.net
open Argu
open NavySearch.Message
open NavySearch.Message.Parser
open NavySearch.Data
open NavySearch.Algolia
open NavySearch.CommandLine

DotEnv.Config()

[<CliPrefix(CliPrefix.DoubleDash)>]
[<NoAppSettings>]
type Arguments =
    | [<MainCommand>] Files of FILES: string list
    | [<AltCommandLine("-a")>] All
    | [<AltCommandLine("-h")>] Human_Readable
    | Version
    interface IArgParserTemplate with
        member arg.Usage =
            match arg with
            | All -> "do not ignore entries starting with ."
            | Human_Readable -> "with -l and/or -s, print human readable sizes\n(e.g., 1K 234M 2G)"
            | Version -> "output version information and exit"
            | Files _ -> "File expression to list"

[<EntryPoint>]
let main argv =
    let errorHandler =
        ProcessExiter
            (colorizer =
                function
                | ErrorCode.HelpText -> None
                | _ -> Some ConsoleColor.Red)

    let parser = ArgumentParser.Create<Arguments>(programName = "usn", errorHandler = errorHandler)
    let results = parser.ParseCommandLine argv
    
    let id = getEnvVar "ALGOLIA_APP_ID"
    let key = getEnvVar "ALGOLIA_ADMIN_API_KEY"

    id |> info
    key |> info

    0 // return an integer exit code
