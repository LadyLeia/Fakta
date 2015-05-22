﻿[<AutoOpen>]
module internal Fakta.Prelude

[<assembly: System.Runtime.CompilerServices.InternalsVisibleTo "Fakta.Tests">]
()

let flip f a b = f b a

module Duration =
  open NodaTime
  open System.Diagnostics

  let fromStopwatch (sw : Stopwatch) =
    Duration.FromTimeSpan (sw.Elapsed)

  let time f =
    let sw = Stopwatch.StartNew()
    let res = f ()
    sw.Stop()
    res, fromStopwatch sw

  let timeAsync f =
    async {
      let sw = Stopwatch.StartNew()
      let! res = f ()
      sw.Stop()
      return res, fromStopwatch sw
    }

  let consulString (d : Duration) =
    sprintf "%f%s" (d.ToTimeSpan().TotalSeconds) "s"

module UTF8 =
  open System.Text

  let toString (bs : byte []) =
    Encoding.UTF8.GetString bs

open System

type Random with
  /// generate a new random ulong64 value
  member x.NextUInt64() =
    let buffer = Array.zeroCreate<byte> sizeof<UInt64>
    x.NextBytes buffer
    BitConverter.ToUInt64(buffer, 0)