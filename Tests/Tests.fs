module Tests

open Xunit
open FsUnit.Xunit
open NavySearch.Common
open NavySearch.Message
open NavySearch.Message.Parser

[<Fact>]
let ``can take letters from a string``() =
    takeLetters "abc123" |> should equal "abc"
    takeLetters "123456" |> should equal ""
    takeLetters "" |> should equal ""

[<Fact>]
let ``can get message type from message code``() =
    getType "NAV" |> should equal NAVADMIN
    getType "ALN" |> should equal ALNAV
    getType "FOO" |> should equal UNKNOWN

[<Fact>]
let ``can get message code from message type``() =
    getCode NAVADMIN |> should equal "NAV"
    getCode ALNAV |> should equal "ALN"
    getCode UNKNOWN |> should equal "NAV"

[<Fact>]
let ``can create message identifiers``() =
    createMessageId NAVADMIN 15 42 |> should equal "NAVADMIN15042"
    createMessageId NAVADMIN 20 105 |> should equal "NAVADMIN20105"
    createMessageId ALNAV 16 42 |> should equal "ALNAV16042"
    createMessageId ALNAV 20 3 |> should equal "ALNAV20003"
    createMessageId UNKNOWN 15 42 |> should equal "NAVADMIN15042"
    createMessageId UNKNOWN 20 105 |> should equal "NAVADMIN20105"

[<Fact>]
let ``can create NPC URL from type and year``() =
    createNpcPageUrl NAVADMIN 15
    |> should equal "http://www.public.navy.mil/bupers-npc/reference/messages/NAVADMINS/Pages/NAVADMIN2015.aspx"
    createNpcPageUrl NAVADMIN 20
    |> should equal "http://www.public.navy.mil/bupers-npc/reference/messages/NAVADMINS/Pages/NAVADMIN2020.aspx"
    createNpcPageUrl ALNAV 15
    |> should equal "http://www.public.navy.mil/bupers-npc/reference/messages/ALNAVS/Pages/ALNAV2015.aspx"
    createNpcPageUrl ALNAV 20
    |> should equal "http://www.public.navy.mil/bupers-npc/reference/messages/ALNAVS/Pages/ALNAV2020.aspx"

[<Fact>]
let ``can create message URL fragment``() =
    createMessageUriFragment NAVADMIN 15 42
    |> should equal "bupers-npc/reference/messages/Documents2/NAV2015/NAV15042.txt"
    createMessageUriFragment NAVADMIN 20 3
    |> should equal "bupers-npc/reference/messages/Documents/NAVADMINS/NAV2020/NAV20003.txt"
    createMessageUriFragment NAVADMIN 20 93
    |> should equal "bupers-npc/reference/messages/Documents/NAVADMINS/NAV2020/NAV20093.txt"
    createMessageUriFragment ALNAV 15 9
    |> should equal "bupers-npc/reference/messages/Documents/ALNAVS/ALN2015/ALN15009.txt"
    createMessageUriFragment ALNAV 20 123
    |> should equal "bupers-npc/reference/messages/Documents/ALNAVS/ALN2020/ALN20123.txt"

[<Fact>]
let ``can get current year in YY format``() = getCurrentYear() |> should equal 20

[<Fact>]
let ``can creat string output of years``() =
    createYearsString [ 16 ] |> should equal "16"
    createYearsString [ 20; 18 ] |> should equal "18 and 20"
    createYearsString [ 20; 15; 18 ] |> should equal "15, 18, and 20"

[<Fact>]
let ``can parse message identifiers``() =
    parseMessageIdentifier "NAVADMIN15042"
    |> should equal
           { MessageType = NAVADMIN
             Number = 42
             Year = 15
             Text = "" }
    parseMessageIdentifier "ALNAV20123"
    |> should equal
           { MessageType = ALNAV
             Number = 123
             Year = 20
             Text = "" }
    parseMessageIdentifier "FOOBAR20123"
    |> should equal
           { MessageType = UNKNOWN
             Number = 123
             Year = 20
             Text = "" }
    parseMessageIdentifier "NAV15001"
    |> should equal
           { MessageType = NAVADMIN
             Number = 1
             Year = 15
             Text = "" }
    parseMessageIdentifier "ALN19123"
    |> should equal
           { MessageType = ALNAV
             Number = 123
             Year = 19
             Text = "" }

[<Fact>]
let ``can parse message URI``() =
    parseMessageUri "bupers-npc/reference/messages/Documents2/NAV2015/NAV15042.txt"
    |> should equal
           { MessageType = NAVADMIN
             Number = 42
             Year = 15
             Text = "" }
    parseMessageUri "/bupers-npc/reference/messages/Documents/NAVADMINS/NAV2020/nav20093.txt"
    |> should equal
           { MessageType = NAVADMIN
             Number = 93
             Year = 20
             Text = "" }

[<Fact>]
let ``can chunk record text property``() =
    let data =
        { MessageType = NAVADMIN
          Number = 42
          Year = 15
          Text = "123123" }
    chunkByTextValue data 3
    |> Seq.iter (fun chunk ->
        chunk
        |> should equal
               { MessageType = NAVADMIN
                 Number = 42
                 Year = 15
                 Text = "123" })
    chunkByTextValue data 6
    |> Seq.iter (fun chunk ->
        chunk
        |> should equal
               { MessageType = NAVADMIN
                 Number = 42
                 Year = 15
                 Text = "123123" })

[<Fact>]
let ``can parse message text``() =
    let text = """
        UNCLASSIFIED//

        ROUTINE

        R 152131Z MAY 20 MID110000692205U

        FM CNO WASHINGTON DC

        TO NAVADMIN

        INFO CNO WASHINGTON DC

        BT
        UNCLAS

        NAVADMIN 144/20

        MSGID/GENADMIN/CNO WASHINGTON DC/N1/MAY//

        SUBJ/RECOMMENCEMENT OF SELECTION BOARDS AND ANNOUNCEMENT OF REVISED
        SCHEDULE//
    """

    let data =
        { MessageType = NAVADMIN
          Number = 42
          Year = 15
          Text = text }
    getSectionContent "subject" data.Text
    |> should equal "RECOMMENCEMENT OF SELECTION BOARDS AND ANNOUNCEMENT OF REVISED SCHEDULE"

[<Fact>]
let ``can parse message classification``() = unwrap classification "UNCLASSIFIED//" |> should equal "UNCLASSIFIED"

[<Fact>]
let ``can parse message header``() =
    let text = """

        ROUTINE

        R 152131Z MAY 20 MID110000692205U

        FM CNO WASHINGTON DC

        TO NAVADMIN

        INFO CNO WASHINGTON DC

        BT
        UNCLAS

        NAVADMIN 144/20

        MSGID/GENADMIN/CNO WASHINGTON DC/N4/APR//
    """
    true |> should equal true

[<Fact>]
let ``can parse message ID``() =
    let messageId = "MSGID/GENADMIN/CNO WASHINGTON DC/N1/MAY//"
    unwrap (sectionContent "messageId") messageId |> should equal "GENADMIN/CNO WASHINGTON DC/N1/MAY"

[<Fact>]
let ``can parse section identifiers``() = unwrap sectionIdentifier "SUBJ/foobar" |> should equal "SUBJ/"

[<Fact>]
let ``can parse message subjects``() =
    unwrap subject "SUBJ/foobarbaz//" |> should equal "foobarbaz"

    let subjectWithNewline = """
        SUBJ/RECOMMENCEMENT OF SELECTION BOARDS AND ANNOUNCEMENT OF REVISED
        SCHEDULE//
    """
    let subjectWithMultipleNewlines = """
        SUBJ/GUIDANCE ON EVALUATION OF DEPLOYABILITY, TEMPORARY LIMITED DUTY, AND
        REFERRAL TO THE DISABILITY EVALUATION SYSTEM (DES) DURING THE CORONAVIRUS
        (COVID-19) PANDEMIC//
    """
    unwrap subject subjectWithNewline
    |> should equal "RECOMMENCEMENT OF SELECTION BOARDS AND ANNOUNCEMENT OF REVISED SCHEDULE"
    unwrap subject subjectWithMultipleNewlines
    |> should equal
           "GUIDANCE ON EVALUATION OF DEPLOYABILITY, TEMPORARY LIMITED DUTY, AND REFERRAL TO THE DISABILITY EVALUATION SYSTEM (DES) DURING THE CORONAVIRUS (COVID-19) PANDEMIC"
