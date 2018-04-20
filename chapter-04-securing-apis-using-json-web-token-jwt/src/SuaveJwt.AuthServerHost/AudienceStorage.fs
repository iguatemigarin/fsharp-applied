namespace SuaveJwt.AuthServerHost

open System.Collections.Generic
open SuaveJwt.JwtToken

module AudienceStorage =
    let private audienceStorage = new Dictionary<string, Audience>()
    let saveAudience (audience: Audience) =
        audienceStorage.Add(audience.ClientId, audience)
        audience |> async.Return
    