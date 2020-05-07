module Tests

open Xunit
open FsUnit.Xunit

open NavySearch.Common

[<Fact>]
let ``can get message type from message code`` () =
    getType "NAV" |> should equal NAVADMIN
    getType "ALN" |> should equal ALNAV
    getType "FOO" |> should equal UNKNOWN