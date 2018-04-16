namespace SuaveJwt

open System
open System.Security.Cryptography
open SuaveJwt.Encodings

module JwtToken =
    type Audience = {
        ClientId: string
        Name: string
        Secret: Base64String
    }

    let createAudience audienceName =
        let clientId = Guid.NewGuid().ToString()
        let data = Array.zeroCreate 32
        RNGCryptoServiceProvider.Create().GetBytes(data)
        let secret = data |> Base64String.Create
        { ClientId = clientId; Secret = secret; Name = audienceName}
