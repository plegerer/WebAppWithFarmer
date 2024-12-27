#r "nuget:Farmer"

open Farmer
open Farmer.Builders

let publishPath = $"{__SOURCE_DIRECTORY__}/bin/Release/net8.0/publish"

if not (System.IO.Directory.Exists publishPath) then
    failwith "The application has not been published for deployment. Run the following command first: dotnet publish -c Release"

let web =
    webApp {
        name "apitestoervapi-westeurope-01" // must be globally unique! url will be this + .azurewebsites.net
        operating_system Linux
        sku WebApp.Sku.F1
        runtime_stack Runtime.DotNet80
        zip_deploy publishPath
    }

let deployment =
    arm {
        location Location.WestEurope
        add_resource web
    }

deployment
|> Deploy.execute "fsharpwebapponlinux" Deploy.NoParameters