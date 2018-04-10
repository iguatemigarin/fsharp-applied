namespace SuaveRestApi.Rest

open Suave
open Suave.RequestErrors
open Suave.Operators
open Suave.Successful
open Suave.Filters
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

[<AutoOpen>]
module RestFul =
    type RestResource<'a> = {
        GetAll : unit -> 'a seq
        Create : 'a -> 'a
        Update : 'a -> 'a option
    }

    let JSON v =
        let settings = new JsonSerializerSettings()
        settings.ContractResolver <- new CamelCasePropertyNamesContractResolver()

        JsonConvert.SerializeObject(v, settings)
        |> OK >=> Writers.setMimeType "application/json"
    let fromJson<'a> json =
        JsonConvert.DeserializeObject(json, typeof<'a>) :?> 'a
    let getResourceFromRequest<'a> (req: HttpRequest) =
        let getString rawForm = System.Text.Encoding.UTF8.GetString(rawForm)
        req.rawForm |> getString |> fromJson<'a>

    let rest resourceName resource =
        let resourcePath = "/" + resourceName
        let getAll = warbler (fun _ ->
            resource.GetAll () |> JSON)
        let badRequest = BAD_REQUEST "Resource not found"

        let handleResource requestError = function
            | Some r -> r |> JSON
            | _ -> requestError
        
        path resourcePath >=> choose [
            GET >=> getAll
            POST >=>
                request (getResourceFromRequest >> resource.Create >> JSON)
            PUT >=>
                request (getResourceFromRequest >> resource.Update >> handleResource badRequest)
        ]
