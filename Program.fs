module FreeFinanceApi.Program

open Falco
open Falco.Routing
open Falco.HostBuilder
open Microsoft.AspNetCore.Builder

type Person =
    { FirstName : string
      LastName : string }

let mapQueryHandler : HttpHandler =
    Request.mapQuery (fun q ->
        let first = q.GetString ("FirstName", "John") // Get value or return default value
        let last = q.GetString ("LastName", "Doe")
        { FirstName = first; LastName = last })
        Response.ofJson

webHost [||] {
    use_https
    endpoints [
        get "/" (Response.ofPlainText "Hello World!")
        get "/callback/" mapQueryHandler
    ]
}