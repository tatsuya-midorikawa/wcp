module Wcp

open System
open System.Linq
open System.Web
open System.Text
open System.Collections.Generic
open System.Reflection

type internal dict<'key, 'value> = Dictionary<'key, 'value>
type Context = { cmd: string; param: string }

type ProtocolAttribute (name: string) =
  inherit Attribute()
  member val name = name with get

let private proto'type = typeof<ProtocolAttribute>
let inline private get'customattrs<'a> (mi: MethodInfo) = mi.GetCustomAttributes(typeof<'a>, false).OfType<'a>().ToArray()
let inline private fst'customattr<'a> (mi: MethodInfo) = (get'customattrs<'a> mi).FirstOrDefault()

[<AbstractClass>]
type Command (args: string[], ?is'fsx: bool) =
  let args' = args
  let raw' =
    let parameter =
      match is'fsx with
      | Some is'fsx -> 
        if is'fsx
        then if 1 < args.Length then args.[1] else ""
        else ""
      | None -> args.FirstOrDefault()
    HttpUtility.UrlDecode(parameter, UTF8Encoding(false))
  let ctx' = 
    let idx = raw'.IndexOf ":"
    let cmd = if 0 <= idx then raw'.Substring(0, idx) else ""
    let param = if 0 <= idx then raw'.Substring(idx + 1) else ""
    { cmd = cmd; param = param }

  member __.args = args'
  member __.raw = raw'
  member __.ctx = ctx'
  member __.run() =
    let method = 
      __.GetType()
        .GetMethods()
        .FirstOrDefault(fun m -> m.IsDefined(proto'type, false) && (m |> fst'customattr<ProtocolAttribute>).name = ctx'.cmd)
    if method <> Unchecked.defaultof<_>
    then Ok (method.Invoke(__, null))
    else Error $"Not found protocol: {ctx'.cmd}"
    

let run<'T> (args: string[]) =
  0