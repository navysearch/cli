module Tests

open Xunit
open FsUnit.Xunit
open NavySearch.Common
open NavySearch.Message

[<Fact>]
let ``can create NPC URL from type and year`` () =
    createNpcPageUrl NAVADMIN 15 |> should equal "http://www.public.navy.mil/bupers-npc/reference/messages/NAVADMINS/Pages/NAVADMIN2015.aspx"
    createNpcPageUrl NAVADMIN 20 |> should equal "http://www.public.navy.mil/bupers-npc/reference/messages/NAVADMINS/Pages/NAVADMIN2020.aspx"
    createNpcPageUrl ALNAV 15 |> should equal "http://www.public.navy.mil/bupers-npc/reference/messages/ALNAVS/Pages/ALNAV2015.aspx"
    createNpcPageUrl ALNAV 20 |> should equal "http://www.public.navy.mil/bupers-npc/reference/messages/ALNAVS/Pages/ALNAV2020.aspx"

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