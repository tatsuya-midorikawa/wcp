module Wcp

open System.Linq

[<AbstractClass>]
type Command (args: string[], ?is'fsx: bool) =
  let raw = args
  let context =
    match is'fsx with
    | Some is'fsx -> 
      if is'fsx
      then if 1 < args.Length then args[1] else ""
      else ""
    | None -> args.FirstOrDefault()

let run<'T> (args: string[]) =
  0