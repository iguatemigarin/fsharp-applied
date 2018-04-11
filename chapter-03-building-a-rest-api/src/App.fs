open Suave.Web
open Suave.Successful

open SuaveRestApi.Rest
open SuaveRestApi.Db


module Main =
    [<EntryPoint>]
    let main argv =
        let personWebPart = rest "people" {
            GetAll = Db.getPeople
            Create = Db.createPerson
            Update = Db.updatePerson
            Delete = Db.deletePerson
            GetById = Db.getPerson
            UpdateById = Db.updatePersonById
            IsExists = Db.isExist
        }
        startWebServer defaultConfig personWebPart
        0 // return an integer exit code
