module MiniSuave
open Suave.Http
open Suave.Console
open Suave.Successfull
open Suave.Combinators
open Suave.Filters
open System

[<EntryPoint>]
let main argv =
    let request = {Route = ""; Type = Suave.Http.GET}
    let response = {Content = ""; StatusCode = 200}
    let context = {Request = request; Response = response}
    let app = Choose [
                GET >=> Path "/hello" >=> OK "Hello"
                POST >=> Path "/hello" >=> CREATED "Hello"
                Path "/foo" >=> Choose [
                    GET >=> OK "foo"
                    POST >=> CREATED "foo"
                ]
            ]
    executeInLoop context app
    0
