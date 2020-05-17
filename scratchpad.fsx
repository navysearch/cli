
#I @"packages\FParsec\lib\net45"
#r @"FParsecCS.dll"
#r @"FParsec.dll"
#r @"Library\bin\Debug\netstandard2.0\Library.dll"

open NavySearch.Common
open NavySearch.Message
open NavySearch.Message.Parser

["foo"; "bar"] |> List.reduce join

"     " |> removeExtraSpaces