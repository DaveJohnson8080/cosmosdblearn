namespace Model

open System
open FSharp.CosmosDb
open System.Text.Json.Serialization

type DocPart = {
    [<JsonPropertyName("id")>]
    Id : string
    PartOfDoc : string
    [<JsonPropertyName("docId")>]
    DocId : string
    StartIndex : int
    EndIndex : int
    Text : string
}


