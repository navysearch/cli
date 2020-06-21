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

open dotenv.net
open NavySearch.Algolia
open NavySearch.CommandLine

DotEnv.Config()
let id = getEnvVar "ALGOLIA_APP_ID"
let key = getEnvVar "ALGOLIA_ADMIN_API_KEY"

let data = getAllMessageData id key

data |> Seq.length