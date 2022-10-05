module Tests

open System
open Xunit
open Xunit.Abstractions

type Foo = {
  raw: string[]
} 
with 
  member __.foo() = 0

type Bar () =
  new (x: int) = Bar()

type LoggerTest (output: ITestOutputHelper) =
  let log msg = output.WriteLine $"{msg}"

  [<Fact>]
  member __.``My test`` () =
    Assert.True(true)

  [<Fact>]
  member __.``exp`` () =
    let t = typedefof<Bar>
    t.GetConstructors()
    |> Array.iter log
    Assert.True(true)
    //t.GetConstructors()
    //|> Array.iter (fun x -> 
    //  x.GetParameters()
    //  |> Array.iter (fun y -> y.Name |> log) )
    //Assert.True(true)
