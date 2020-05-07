namespace Library

module Say =
    let hello name =
        printfn "Hello %s" name
        sprintf "Ohayo %s san" name
