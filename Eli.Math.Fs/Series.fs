namespace Eli.Math.Fs

module Series =
    open System

    let seriesExpansion f x0 n h =
        let terms =
            [for k in 0 .. n - 1 do
                let term = (Derivative.derivative f x0 h Derivative.Method.Default) / float (Sequence.factorial k)
                yield fun x -> term * Math.Pow(x - x0, float k) ]
        fun x -> terms |> List.sumBy (fun term -> term x)
