module Tests

open Xunit
open FsUnit.Xunit

open NavySearch.Common
open NavySearch.Message

[<Fact>]
let ``can get current year in YY format`` () =
    getCurrentYear() |> should equal 20

[<Fact>]
let ``can take letters from a string`` () =
    takeLetters "abc123" |> should equal "abc"
    takeLetters "123456" |> should equal ""
    takeLetters "" |> should equal ""

[<Fact>]
let ``can get message type from message code`` () =
    getType "NAV" |> should equal NAVADMIN
    getType "ALN" |> should equal ALNAV
    getType "FOO" |> should equal UNKNOWN