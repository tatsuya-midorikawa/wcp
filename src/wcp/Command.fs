module Wcp

open System.Linq
open System.Web
open System.Text

type Context = { cmd: string; param: string }

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

let run<'T> (args: string[]) =
  0