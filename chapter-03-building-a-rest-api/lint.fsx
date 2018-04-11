#r @"packages/FAKE/tools/FakeLib.dll"
open Fake

#I @"packages/FSharpLint.Fake/tools"
#r @"packages/FSharpLint.Fake/tools/FSharpLint.Fake.dll"
open FSharpLint.Fake

Target "Lint" (fun _ ->
    !! "src/**/*.fsproj"
        |> Seq.iter (FSharpLint id))

RunTargetOrDefault "Lint"