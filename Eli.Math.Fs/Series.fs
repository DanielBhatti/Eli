namespace Eli.Math.Fs

module Series =
    open System

    // Helper method to calculate factorial
    let rec factorial n =
        if n = 0 then 1
        else n * factorial (n - 1)

    // Helper method to calculate the k-th derivative of f at x0
    let derivative f x0 h k =
        // Central difference method for simplicity
        let rec aux f x0 h k =
            if k = 0 then f x0
            else (aux f (x0 + h) h (k - 1) - aux f (x0 - h) h (k - 1)) / (2.0 * h)
        aux f x0 h k

    // Series expansion of f at x0 up to n terms with step size h
    let seriesExpansion f x0 n h =
        let terms k x = 
            let term = derivative f x0 h k / float (factorial k)
            term * Math.Pow(x - x0, float k)
        fun x -> [0 .. n - 1] |> List.sumBy (terms >> (fun g -> g x))

    // Additional useful methods
    // Taylor series expansion of sin function at 0
    let sinExpansion x n h = seriesExpansion Math.Sin 0.0 n h x

    // Taylor series expansion of cos function at 0
    let cosExpansion x n h = seriesExpansion Math.Cos 0.0 n h x

    // Taylor series expansion of exp function at 0
    let expExpansion x n h = seriesExpansion Math.Exp 0.0 n h x
