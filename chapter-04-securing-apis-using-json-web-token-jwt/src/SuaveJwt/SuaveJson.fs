namespace SuaveJwt

open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Suave
open Suave.Operators
open Suave.Successful

module SuaveJson =
    let JSON v = 
        let settings = new JsonSerializerSettings()
        settings.ContractResolver <- CamelCasePropertyNamesContractResolver()
        JsonConvert.SerializeObject(v, settings)
        |> OK
        >=> Writers.setMimeType "application/json; charset=utf-8"
    
    let mapJsonPayload<'a> (req: HttpRequest) =
        let fromJson json =
            try
                JsonConvert.DeserializeObject(json, typeof<'a>)
                :?> 'a
                |> Some
            with
            | _ -> None
        
        let getString = System.Text.Encoding.UTF8.GetString

        req.rawForm |> getString |> fromJson
