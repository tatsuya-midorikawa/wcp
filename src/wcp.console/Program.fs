type FooCommand (args: string[]) =
  inherit Wcp.Command(args)
  [<Wcp.Protocol(name = "proto")>]
  member __.foo() =
    printfn $"{__.ctx}"
  [<Wcp.Protocol(name = "proto2")>]
  member __.foo(a: string, b: int) =
    printfn $"{__.ctx}"
  [<Wcp.Protocol(name = "proto3")>]
  member __.foo(p: {| a: string; b: int |}) =
    printfn $"{__.ctx}"

// FooCommand([| "proto://test%5Cfoo%5Cbar" |]).run() |> (printfn "%A")
// FooCommand([| "proto://test%5Cfoo%5Cbar?p1=abc&p2=def" |]).run() |> (printfn "%A")

// let cmd = FooCommand([| "proto://test%5Cfoo%5Cbar" |])

// cmd.ctx |> printfn "%A"

// let s1 = "foo"
// let s2 = "foo:a"

// s1.IndexOf(":") |> printfn "s1: %d"
// s2.IndexOf(":") |> printfn "s2: %d"

// s2.Substring(0, s2.IndexOf(":")) |> printfn "s2 substring: %s"

// open System.Linq

// type Foo () =
//   [<Wcp.Protocol(name = "abc")>]
//   member __.a() = ()
//   member __.b() = ()

let foo = FooCommand([| "proto://test%5Cfoo%5Cbar" |])
let ty = foo.GetType()
let methods =
  ty.GetMethods()
  |> Array.filter (fun m -> m.IsDefined(typeof<Wcp.ProtocolAttribute>, false))

methods
|> Array.iter (fun m -> 
  let ps = m.GetParameters()
  printfn $"{m}"
  ps |> Array.iter (fun p -> printfn $"{p.ParameterType}: {p.Name} ({p.Position})")
  )
// methods
// |> Array.filter (fun m -> m.IsDefined(typeof<Wcp.ProtocolAttribute>, false))
// |> Array.iter (fun m -> 
//   let protocol = m.GetCustomAttributes(typeof<Wcp.ProtocolAttribute>, false)[0] :?> Wcp.ProtocolAttribute
//   printfn $"{m.Name}: {protocol.name}")