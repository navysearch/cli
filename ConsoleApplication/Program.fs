// Learn more about F# at http://fsharp.org

open System
open dotenv.net
open Argu
open NavySearch.Message
open NavySearch.Data
open NavySearch.CommandLine

DotEnv.Config()

[<EntryPoint>]
let main argv =
    // let url = "https://www.public.navy.mil/bupers-npc/reference/messages/NAVADMINS/Pages/NAVADMIN2020.aspx"
    // let results = scrapeMessageLinks url
    // printf "%A" results
    printfn "id: %A" (Environment.GetEnvironmentVariable("ALGOLIA_APP_ID"))
    printfn "key: %A" (Environment.GetEnvironmentVariable("ALGOLIA_ADMIN_API_KEY"))
    let errorHandler =
        ProcessExiter
            (colorizer =
                function
                | ErrorCode.HelpText -> None
                | _ -> Some ConsoleColor.Red)

    let parser = ArgumentParser.Create<Arguments>(programName = "usn", errorHandler = errorHandler)
    let results = parser.ParseCommandLine argv
    printfn "Got parse results %A" <| results.GetAllResults()
    let output =
        match results with
        | p when p.Contains(Download) -> "Download in progress..."
        | _ -> "Nothing ventured, nothing gained"
    warn <| output
    0 // return an integer exit code
