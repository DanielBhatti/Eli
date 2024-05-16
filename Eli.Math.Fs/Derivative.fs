namespace Eli.Math.Fs

module Derivative =
    type Method =
        | Default
        | Forward
        | Backward
        | Central

    let derivative (f: float -> float) x delta method =
        match method with
        | Forward -> (f (x + delta) - f x) / delta
        | Backward -> (f x - f (x - delta)) / delta
        | Central | Default -> (f (x + delta) - f (x - delta)) / (2.0 * delta)

    let rec nthDerivative (f: float -> float) x n delta method =
        if n = 0 then f x
        else if n = 1 then derivative f x delta method
        else
            let g = (fun y -> nthDerivative f y (n - 1) delta method)
            derivative g x delta method

    let gradient (f: float[] -> float) (point: float[]) delta method =
        point
        |> Array.mapi (fun i x_i ->
            let shiftedPoint h = point |> Array.mapi (fun j x_j -> if i = j then x_j + h else x_j)
            let fShifted h = f (shiftedPoint h)
            match method with
            | Forward -> (fShifted delta - f point) / delta
            | Backward -> (f point - fShifted (-delta)) / delta
            | Central | Default -> (fShifted delta - fShifted (-delta)) / (2.0 * delta)
        )

    let jacobian (f: float[] -> float[]) (point: float[]) delta =
        let m = f point |> Array.length
        let n = point |> Array.length
        Array.init m (fun i ->
            Array.init n (fun j ->
                let shiftedPoint h = point |> Array.mapi (fun k x_k -> if j = k then x_k + h else x_k)
                let fShifted h = f (shiftedPoint h)
                ((fShifted delta).[i] - (fShifted (-delta)).[i]) / (2.0 * delta)
            )
        )