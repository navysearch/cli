// Learn more about F# at http://fsharp.org

open System
open dotenv.net
open Argu
open NavySearch.Message
open NavySearch.Data

DotEnv.Config()

type OpenArguments =
    | [<AltCommandLine("-a")>] App of name:string
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | App _ -> "Name of application to open (ex: nsips)."
and DownloadArguments =
    | [<AltCommandLine("-t")>] Type of str:string
    | [<AltCommandLine("-y")>] Year of YY:int
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Type _ -> "Message type to download: nav (NAVADMIN) or aln (ALNAV)."
            | Year _ -> "Year to download messages from."
and Arguments =
    | Version
    | [<AltCommandLine("-v")>] Verbose
    | [<CliPrefix(CliPrefix.None)>] Open of ParseResults<OpenArguments>
    | [<CliPrefix(CliPrefix.None)>] Download of ParseResults<DownloadArguments>
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Version -> "Print the version."
            | Verbose -> "Print a lot of output to stdout."
            | Open _ -> "Perform a string search and return results."
            | Download _ -> "Download messages for a given year."

[<EntryPoint>]
let main argv =
    // let url = "https://www.public.navy.mil/bupers-npc/reference/messages/NAVADMINS/Pages/NAVADMIN2020.aspx"
    // let results = scrapeMessageLinks url
    // printf "%A" results
    printfn "id: %A" (Environment.GetEnvironmentVariable("ALGOLIA_APP_ID"))
    printfn "key: %A" (Environment.GetEnvironmentVariable("ALGOLIA_ADMIN_API_KEY"))
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let parser = ArgumentParser.Create<Arguments>(programName = "usn", errorHandler = errorHandler)
    let results = parser.ParseCommandLine argv
    printfn "Got parse results %A" <| results.GetAllResults()
    Console.ForegroundColor <- ConsoleColor.Blue
    let output =
        match results with
        | p when p.Contains(Download) -> "Download in progress..."
        | _ -> "Nothing ventured, nothing gained"
    printfn "%s" <| output
    0 // return an integer exit code