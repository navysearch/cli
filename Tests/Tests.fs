module Tests

open System
open FsUnit.Xunit
open Xunit

open Library.Say

[<Fact>]
let ``My test`` () =
    Assert.True(true)

[<Fact>]
let ``Libary Smoke Test`` () =
    hello "Tanaka" |> should equal "Ohayo Tanaka san"