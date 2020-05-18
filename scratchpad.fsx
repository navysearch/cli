#I @"packages\FParsec\lib\net45"
#I @"packages\Fsharp.Data\lib\net45"
#r "FParsecCS.dll"
#r "FParsec.dll"
#r "FSharp.Data.dll"
#r @"Library\bin\Debug\netstandard2.0\Library.dll"


open NavySearch.Common
open NavySearch.Data
open NavySearch.Message
open NavySearch.Message.Parser

scrapeMessageLinks NAVADMIN 20
|> List.skip 10
|> List.take 50
|> List.map
    (parseMessageUri
     >> getMessage
     >> (getSectionContent "subject"))
