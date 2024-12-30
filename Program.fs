module FreeFinanceApi.Program

open Falco
open Falco.Routing
open Falco.HostBuilder
open Microsoft.AspNetCore.Builder

type Person =
    { FirstName : string
      LastName : string }

let timer = async {
    printfn "Start"
    do! Async.Sleep 10000
    printfn "End"
}

let test  : HttpHandler = fun ctx -> 
    task{
        printfn "TEST"
        timer |> Async.StartAsTask |> ignore
        return! Response.ofPlainText "mein Test" ctx
    }
    

let mapQueryHandler : HttpHandler =
    Request.mapQuery (fun q ->
        let first = q.GetString ("FirstName", "John") // Get value or return default value
        let last = q.GetString ("LastName", "Doe")
        { FirstName = first; LastName = last })
        Response.ofJson

let jsonHandler : HttpHandler =
    { FirstName = "John"
      LastName = "Doe" }
    |> Response.ofJson

let mapJsonHandler : HttpHandler =
 
    let handleOk (person:Person) : HttpHandler =
        let message = sprintf "hello %s %s" person.FirstName person.LastName
        Response.ofPlainText message

    Request.mapJson handleOk

webHost [||] {
    use_https
    endpoints [
        get "/" (Response.ofPlainText "Hello World!")
        get "/callback/" mapQueryHandler
        put "/command-test/" jsonHandler
        put "/command-test2/" test
    ]
}