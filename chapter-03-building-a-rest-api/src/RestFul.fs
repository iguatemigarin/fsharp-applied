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
        Delete : int -> unit
        GetById : int -> 'a option
        UpdateById : int -> 'a -> 'a option
        IsExists : int -> bool
    }

    let JSON v =
        let settings = new JsonSerializerSettings ()
        settings.ContractResolver <- new CamelCasePropertyNamesContractResolver ()

        JsonConvert.SerializeObject(v, settings)
        |> OK >=> Writers.setMimeType "application/json"
    let fromJson<'a> json =
        JsonConvert.DeserializeObject(json, typeof<'a>) :?> 'a
    let getResourceFromRequest<'a> (req: HttpRequest) =
        let getString rawForm = System.Text.Encoding.UTF8.GetString rawForm
        req.rawForm |> getString |> fromJson<'a>

    let rest resourceName resource =
        let resourcePath = "/" + resourceName
        
        let getAll = warbler (fun _ ->
            resource.GetAll () |> JSON)
        
        let badRequest = BAD_REQUEST "Resource not found"

        let handleResource requestError = function
            | Some r -> r |> JSON
            | _ -> requestError
        
        let resourceIdPath =
            let path = resourcePath + "/%d"
            new PrintfFormat<(int -> string), unit, string, string, int>(path)
        
        let deleteResourceById id =
            resource.Delete id
            NO_CONTENT
        
        let getResourceById =
            resource.GetById >> handleResource (NOT_FOUND "Resource not found")
        
        let updateResourceById id =
            request (getResourceFromRequest >> (resource.UpdateById id) >> handleResource badRequest)

        let isResourceExists id =
            if resource.IsExists id then OK "" else NOT_FOUND ""
        
        choose [
            path resourcePath >=> choose [
                GET >=> getAll
                POST >=>
                    request (getResourceFromRequest >> resource.Create >> JSON)
                PUT >=>
                    request (getResourceFromRequest >> resource.Update >> handleResource badRequest)
            ]
            DELETE >=> pathScan resourceIdPath deleteResourceById
            GET >=> pathScan resourceIdPath getResourceById
            PUT >=> pathScan resourceIdPath updateResourceById
            HEAD >=> pathScan resourceIdPath isResourceExists
        ]
