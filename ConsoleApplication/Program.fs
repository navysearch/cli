// Learn more about F# at http://fsharp.org

open System
open NavySearch.Data

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    // let url = "https://www.public.navy.mil/bupers-npc/reference/messages/NAVADMINS/Pages/NAVADMIN2020.aspx"
    // let results = scrapeMessageLinks url
    // printf "%A" results
    printf "%A" argv
    0 // return an integer exit code
