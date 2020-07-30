#time
#I @"packages\Algolia.Search\lib\netstandard2.0"
#I @"packages\FParsec\lib\net45"
#I @"packages\Fsharp.Data\lib\net45"
#I @"packages\dotenv.net\lib\netstandard2.0"
#I @"packages\Newtonsoft.Json\lib\netstandard2.0"
#r "Algolia.Search.dll"
#r "FParsecCS.dll"
#r "FParsec.dll"
#r "FSharp.Data.dll"
#r "dotenv.net.dll"
#r @"Library\bin\Debug\netstandard2.0\Library.dll"

open System
open System.Net

open dotenv.net
open NavySearch.Algolia
open NavySearch.CommandLine
open NavySearch.Data
open NavySearch.Message
open NavySearch.Message.Parser

// DotEnv.Config()
// let id = getEnvVar "ALGOLIA_APP_ID"
// let key = getEnvVar "ALGOLIA_ADMIN_API_KEY"

// let messages = getMessageDataByYear NAVADMIN 20
// messages |> List.filter (fun x -> x.Subject = "UNINTENTIONALLY LEFT BLANK") |> List.map (fun x -> x.Number)
// let url = Uri "https://www.public.navy.mil/bupers-npc/reference/messages/Documents/NAVADMINS/NAV2020/NAV20203.txt"
// let client = new WebClient()
// let message = client.DownloadFile(url, "NAV20203.txt")

let localPath = @"C:\Users\jason\dev\messages"

let downloaded, failed = DownloadMessages NAVADMIN 20 localPath
failed