namespace test.wcp

open Xunit
open Xunit.Abstractions

type TestCommand (args: string[]) =
  inherit Wcp.Command(args)

type CommandTest (output: ITestOutputHelper) =
  let log msg = output.WriteLine $"{msg}"

  [<Fact>]
  member __.``Command test`` () =
    let cmd = TestCommand([| "proto://test%5Cfoo%5Cbar" |])
    cmd.raw |> log
    cmd.ctx |> log
