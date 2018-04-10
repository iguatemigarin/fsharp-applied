// Learn more about F# at http://fsharp.org

open System
open Suave
open Suave.Operators
open Suave.Successful
open Suave.Filters
open System.IO


[<EntryPoint>]
let main argv =
    let json fileName =
        let content = File.ReadAllText fileName
        content.Replace("\r", "").Replace("\n", "")
        |> OK >=> Writers.setMimeType "application/json"

    let user = pathScan "/users/%s" (fun _ -> "data/User.json" |> json)
    let repos = pathScan "/repos/%s" (fun _ -> "data/Repos.json" |> json)
    let mockApi = choose [user;repos]

    startWebServer defaultConfig mockApi
    0 // return an integer exit code
