namespace Eli.Math.Fs

module Derivative =
    type Method =
        | Default
        | Forward
        | Backward
        | Central

    let derivative (f: float -> float) x delta (method: Method) =
        match method with
        | Forward | Default -> (f (x + delta) - f x) / delta
        | Backward -> (f x - f (x - delta)) / delta
        | Central -> (f (x + delta) - f (x - delta)) / (2.0 * delta)

    let gradient (f: float[] -> float) point delta method =
        point
        |> Array.mapi (fun i x_i ->
            let rightShift = Array.mapi (fun j x_j -> if i = j then x_j + delta else x_j) point
            let leftShift = Array.mapi (fun j x_j -> if i = j then x_j - delta else x_j) point
            match method with
            | Forward | Default -> derivative (fun t -> f rightShift) x_i delta Forward
            | Backward -> derivative (fun t -> f leftShift) x_i delta Backward
            | Central -> derivative (fun t -> f rightShift) x_i delta Central
        )

    let jacobian (f: float[] -> float[]) point delta =
        let n = point |> Array.length
        let m = f point |> Array.length
        Array.init m (fun i ->
            Array.init n (fun j ->
                let rightShift = Array.mapi (fun k x_k -> if k = j then x_k + delta else x_k) point
                let leftShift = Array.mapi (fun k x_k -> if k = j then x_k - delta else x_k) point
                (f rightShift).[i] - (f leftShift).[i] / (2.0 * delta)
            )
        )