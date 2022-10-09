type FooCommand (args: string[]) =
  inherit Wcp.Command(args)

let cmd = FooCommand([| "proto://test%5Cfoo%5Cbar" |])
cmd.ctx |> printfn "%A"

let s1 = "foo"
let s2 = "foo:a"

s1.IndexOf(":") |> printfn "s1: %d"
s2.IndexOf(":") |> printfn "s2: %d"

s2.Substring(0, s2.IndexOf(":")) |> printfn "s2 substring: %s"
System.Diagnostics.Debug.WriteLine("test")