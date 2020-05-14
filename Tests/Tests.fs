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
let ``can create message URL fragment`` () =
    createMessageUrl NAVADMIN 15 42 |> should equal "bupers-npc/reference/messages/Documents2/NAV2015/NAV15042.txt"
    createMessageUrl NAVADMIN 20 3 |> should equal "bupers-npc/reference/messages/Documents/NAVADMINS/NAV2020/NAV20003.txt"
    createMessageUrl ALNAV 15 9 |> should equal "bupers-npc/reference/messages/Documents/ALNAVS/ALN2015/ALN15009.txt"
    createMessageUrl ALNAV 20 123 |> should equal "bupers-npc/reference/messages/Documents/ALNAVS/ALN2020/ALN20123.txt"

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