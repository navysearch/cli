namespace NavySearch

open System
open FSharp.Data
open Algolia.Search.Clients
open Algolia.Search.Models.Search

module Common =
    let getCurrentYear() = (string DateTime.Now.Year).[2..] |> int

    let takeLetters value =
        let letters =
            [ for ch in value -> ch ]
            |> List.toSeq
            |> Seq.takeWhile (Char.IsNumber >> not)
            |> Seq.map string
        match Seq.isEmpty letters with
        | true -> ""
        | false ->
            let join acc item = sprintf "%s%s" acc item
            letters |> Seq.reduce (join)

module Message =
    open Common

    type MessageType =
        | NAVADMIN
        | ALNAV
        | UNKNOWN

    type MessageInfo =
        { MessageType: MessageType
          Number: int
          Year: int
          Text: string }

    let getType code =
        match code with
        | "NAV" -> NAVADMIN
        | "ALN" -> ALNAV
        | _ -> UNKNOWN

    let getCode messageType =
        match messageType with
        | NAVADMIN -> "NAV"
        | ALNAV -> "ALN"
        | UNKNOWN -> "NAV"

    let createMessageId messageType year (messageNumber: int) =
        match messageType with
        | NAVADMIN -> sprintf "NAVADMIN%02i%03i" year messageNumber
        | ALNAV -> sprintf "ALNAV%02i%03i" year messageNumber
        | UNKNOWN -> sprintf "NAVADMIN%02i%03i" year messageNumber

    let createNpcPageUrl messageType (year: int) =
        let npcMessagesUrl = "http://www.public.navy.mil/bupers-npc/reference/messages"

        let yearString =
            match String.length (string year) with
            | x when x = 4 -> (string year).[(x - 2)..]
            | _ -> (string year)
        match messageType with
        | NAVADMIN -> sprintf "%s/NAVADMINS/Pages/NAVADMIN20%s.aspx" npcMessagesUrl yearString
        | ALNAV -> sprintf "%s/ALNAVS/Pages/ALNAV20%s.aspx" npcMessagesUrl yearString
        | UNKNOWN -> sprintf "%s/NAVADMINS/Pages/NAVADMIN20%s.aspx" npcMessagesUrl yearString

    let createMessageUrl messageType year number =
        let code = getCode messageType

        let subfolder =
            match messageType with
            | NAVADMIN when year >= 18 -> "NAVADMINS/"
            | ALNAV -> "ALNAVS/"
            | _ -> ""

        let fragment =
            let bupers = "bupers-npc/reference/messages/Documents"
            match messageType with
            | NAVADMIN when year < 18 -> sprintf "%s2" bupers
            | ALNAV -> bupers
            | _ -> bupers

        sprintf "%s/%s%s20%i/%s%i%03i.txt" fragment subfolder code year code year number

    let createYearsString (years: int list) =
        match List.length (years) with
        | x when x > 2 ->
            let sorted = years |> List.sort

            let intial =
                sorted.[..(x - 2)]
                |> List.map (fun x -> sprintf "%02i, " x)
                |> List.reduce (fun acc item -> sprintf "%s%s" acc item)

            let last =
                sorted
                |> List.toSeq
                |> Seq.last

            sprintf "%sand %i" intial last
        | x when x = 2 -> sprintf "%i and %i" years.Head years.Tail.Head
        | x when x = 1 -> sprintf "%i" years.Head
        | _ -> ""

    let parseMessageIdentifier (value: string) =
        let messageType =
            match takeLetters value with
            | x when String.length (x) = 3 -> getType x
            | "NAVADMIN" -> NAVADMIN
            | "ALNAV" -> ALNAV
            | _ -> UNKNOWN

        let year = value.[(String.length (value) - 5)..(String.length (value) - 4)] |> int
        let messageNumber = int value.[(String.length (value) - 3)..]
        { MessageType = messageType
          Number = messageNumber
          Year = year
          Text = "" }

    let parseMessageUri (value: string) =
        let filename =
            value.Split('/')
            |> Array.toList
            |> List.rev
            |> fun x -> x.Head

        let messageIdentifier =
            filename.Split('.')
            |> Array.toList
            |> fun x -> x.Head

        parseMessageIdentifier messageIdentifier

    let chunkByTextValue (data: MessageInfo) size =
        [ for s in data.Text -> s ]
        |> List.toSeq
        |> Seq.chunkBySize size
        |> Seq.map
            (Seq.toList
             >> List.map (string)
             >> List.reduce (fun acc item -> sprintf "%s%s" acc item)
             >> fun text -> { data with Text = text })

module Data =
    open Message

    let scrapeMessageLinks messageType year =
        let url = (createNpcPageUrl messageType year)
        HtmlDocument.Load(url).Descendants [ "a" ]
        |> Seq.choose (fun x -> x.TryGetAttribute("href") |> Option.map (fun a -> x.InnerText(), a.Value()))
        |> Seq.map (fun (_, href) -> href)
        |> Seq.filter (fun (x: string) -> x.EndsWith(".txt"))
        |> Seq.toList

module Algolia =
    type Hit =
        { objectID: string
          id: string
          year: string
          num: string
          ``type``: string
          code: string
          subject: string
          text: string
          url: string }

    let getMessagesForYear (id: string) (key: string) (year: int) =
        let client = SearchClient(id, key)
        let index = client.InitIndex("message")
        let results = index.Search(Query())
        results.Hits
        |> Seq.map (fun x -> x.subject)
        |> Seq.take 10

module CommandLine =
    let log =
        let lockObj = obj()
        fun color s ->
            lock lockObj (fun _ ->
                Console.ForegroundColor <- color
                printfn "%s" s
                Console.ResetColor())

    let complete = log ConsoleColor.Magenta
    let ok = log ConsoleColor.Green
    let info = log ConsoleColor.Cyan
    let warn = log ConsoleColor.Yellow
    let error = log ConsoleColor.Red
