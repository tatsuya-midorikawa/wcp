// type FooCommand (args: string[]) =
//   inherit Wcp.Command(args)

// let cmd = FooCommand([| "proto://test%5Cfoo%5Cbar" |])
// cmd.ctx |> printfn "%A"

// let s1 = "foo"
// let s2 = "foo:a"

// s1.IndexOf(":") |> printfn "s1: %d"
// s2.IndexOf(":") |> printfn "s2: %d"

// s2.Substring(0, s2.IndexOf(":")) |> printfn "s2 substring: %s"

open System.Linq

type Foo () =
  [<Wcp.Protocol(name = "abc")>]
  member __.a() = ()
  member __.b() = ()

let foo = Foo()
let ty = foo.GetType()
let methods = ty.GetMethods()
methods
|> Array.filter (fun m -> m.IsDefined(typeof<Wcp.ProtocolAttribute>, false))
|> Array.iter (fun m -> 
  let protocol = m.GetCustomAttributes(typeof<Wcp.ProtocolAttribute>, false)[0] :?> Wcp.ProtocolAttribute
  printfn $"{m.Name}: {protocol.name}")