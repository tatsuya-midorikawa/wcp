module Wcp 

open System
open System.Linq
open System.Web
open System.Text
open System.Collections.Generic
open System.Reflection

type internal dict<'key, 'value> = Dictionary<'key, 'value>
// type Context = { cmd: string; q: string; parameters: dict<string, string> }

type public Context = { cmd: string; q: string; parameters: dict<string, string> }

type public ProtocolAttribute (name: string) =
  inherit Attribute()
  member val name = name with get

let private proto'type = typeof<ProtocolAttribute>
let inline private get'customattrs<'a> (mi: MethodInfo) = mi.GetCustomAttributes(typeof<'a>, false).OfType<'a>().ToArray()
let inline private get'protoattr (mi: MethodInfo) = (get'customattrs<ProtocolAttribute> mi).FirstOrDefault()

[<AbstractClass>]
type public Command (args: string[], ?is'fsx: bool) =
  let args' = args
  let raw' =
    match is'fsx with
    | Some is'fsx -> 
      if is'fsx
      then if 1 < args.Length then args.[1] else ""
      else args.FirstOrDefault()
    | None -> args.FirstOrDefault()
  let ctx' =
    let decode (s: string) = HttpUtility.UrlDecode(s, UTF8Encoding(false))
    let idx = raw'.IndexOf ":"
    let cmd = (if 0 <= idx then raw'.Substring(0, idx) else "") |> decode

    let q_raw = if 0 <= idx then raw'.Substring(idx + 1) else ""
    let idx2 = q_raw.IndexOf "?"
    let q = (if 0 <= idx2 then q_raw.Substring(0, idx2) else q_raw) |> decode
    let ps = 
      if 0 <= idx2 
      then
        let acc = dict<string, string>()
        q_raw.Substring(idx2 + 1).Split("&")
        |> Array.iter (fun s -> 
          let x = s.Split("=")
          if x.Length = 2 then acc.Add(decode x[0], decode x[1]))
        acc
      else dict<string, string>()
    { cmd = cmd; q = q; parameters = ps}

  
  [<CompiledName("Args")>]
  member public __.args = args'
  [<CompiledName("Raw")>]
  member public __.raw = raw'
  [<CompiledName("Ctx")>]
  member public __.ctx = ctx'
  [<CompiledName("Run")>]
  member public __.run() =
    let method = 
      __.GetType()
        .GetMethods()
        .FirstOrDefault(fun m -> m.IsDefined(proto'type, false) && (get'protoattr m).name = ctx'.cmd)
    if method <> Unchecked.defaultof<_> && method.GetParameters().Length = 0
    then Ok (method.Invoke(__, null))
    else Error $"Not found protocol: {ctx'.cmd}"

let run<'T> (args: string[]) =
  0