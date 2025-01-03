#r @"nuget: FsHttp"
#r "nuget: FSharp.SystemTextJson"

open FsHttp
open System.Text.Json
open System.Text.Json.Serialization
open System

FsHttp.GlobalConfig.Json.defaultJsonSerializerOptions <-
    let options = JsonSerializerOptions()
    options.Converters.Add(JsonFSharpConverter())
    options

type item = {
    id: string
    name: string
} 

type Items = {items: item list}

type Order = {description: Items}


type transaction = {    
    transactionId: string
    order: Order
    }
type nTransactions = {transactions: transaction list}

let queryTransactions (offset:string) = 
    http {
        GET "https://www.wixapis.com/payments/api/merchant/v2/transactions"

        AuthorizationBearer "IST.eyJraWQiOiJQb3pIX2FDMiIsImFsZyI6IlJTMjU2In0.eyJkYXRhIjoie1wiaWRcIjpcImYxZGEwYzliLTczNDItNDNjMi04ODMyLWExMjFiZmU3NjcxN1wiLFwiaWRlbnRpdHlcIjp7XCJ0eXBlXCI6XCJhcHBsaWNhdGlvblwiLFwiaWRcIjpcImVhYzBmNjJhLTQzNzktNDUyNy1hNWNmLWE3NDgyYjhlMGUwOFwifSxcInRlbmFudFwiOntcInR5cGVcIjpcImFjY291bnRcIixcImlkXCI6XCIxZDcyM2QxMi1kMWNjLTQ1OTMtODQxNi1kYTM0YjNjZmM3OGRcIn19IiwiaWF0IjoxNzM0ODg0Mzk0fQ.Nh5UqBNtWv2m6t5ytGdlETljQZXfhNEhm0VfhQT1cjq9qAoQy6hQnDncpKOFy9q0CzIGV7qU-P-UX5T_T13Duh2xbNG9rXOuRhMWXcQdA47saLgeEC4U9HNn9vTWLPMc0msaAlMgRrv7YNuH32x9eJ96JhLxpZOVXtHaYyNu9KpHxcQNZ98ZcEyIZdAl8BVgH3XxPwe36ItYZghQP3jr7RGpkTsazgvAjc2vZ8-U_hOXycSPoJbe_hOQ_9qsN61_KsZgHEK4q168V1v-MoqjpJlAjg11aiEXUFLcQHNrT3GCsV29hm62R55IArjRqoPrZXGvR2stuR-PSRM3WpmmFg"
        header "wix-site-id" "c68a632d-8c33-475e-b0bd-7481569c21e8"
        query [
            "limit", "100"
            "offset", offset
            ]
    }
    |> Request.send
    |> Response.deserializeJson<nTransactions>

let transform (res:nTransactions) =
    res.transactions 
    |> List.collect (fun y->y.order.description.items)
    //|> List.distinct
    //|> List.iter (fun y -> printfn "%s %s" y.id y.name)

let testq = 
    ["0";"100";"200";"300";"400";"500";"600";"700";"800";"900";"1000"]
    |> List.map queryTransactions
    |> List.map transform
    |> List.collect (fun x -> x)
    |> List.distinct
    |> List.iter (fun y -> printfn "%s %s" y.id y.name)

testq



