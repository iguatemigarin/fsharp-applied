namespace SuaveJwt.AuthServerHost

open SuaveJwt.AuthServer
open Suave.Web

module Main =
    [<EntryPoint>]
    let main argv =
        let authorizationServerConfig = {
            AddAudienceUrlPath = "/api/audience"
            SaveAudience = AudienceStorage.saveAudience
        }
        audienceWebpart authorizationServerConfig
        |> startWebServer defaultConfig
        0 // return an integer exit code
