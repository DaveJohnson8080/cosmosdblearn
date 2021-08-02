// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open FSharp.CosmosDb
open Microsoft.Extensions.Configuration
open System.IO
open Model
open FSharp.Control

// Define a function to construct a message to print
let from whom =
    sprintf "from %s" whom

let getConnection connStr =
    Cosmos.fromConnectionString connStr
    |> Cosmos.database "nlp"
    |> Cosmos.container "docparts"

let storeDocPart connection paragraph =
    async {
        let result =
            connection
            |> Cosmos.upsert<DocPart> paragraph
            |> Cosmos.execAsync
        return result
    }

[<EntryPoint>]
let main argv =
    let builder =
        JsonConfigurationExtensions.AddJsonFile(
            FileConfigurationExtensions.SetBasePath(ConfigurationBuilder(), Directory.GetCurrentDirectory()),
            "appsettings.json",
            true,
            true
        )

    let config = builder.Build()

    let paragraph = {
        Id = "c5944cd2-26c6-4ed8-a8b3-8b93c6bf7652"
        PartOfDoc = "Paragraph"
        DocId = "6dc217cc-4312-4069-b9cb-7da9c38dc1f0"
        StartIndex = 0
        EndIndex = 100
        Text = "Blah"
    }

    let conStr = config.Item("CosmosConnection:ConnectionString")
    let con = 
        conStr
        |> getConnection

    let result =
        paragraph
        |> storeDocPart con
        |> Async.RunSynchronously
        |> AsyncSeq.toListAsync
        |> Async.RunSynchronously

    printfn "%A" result
    0