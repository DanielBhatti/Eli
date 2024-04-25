namespace Eli.Math.Fs

module Dynamics =
    let fixedPoint f (x:float) (tolerance:float) (maxIterations:int) =
        let rec loop current iteration =
            let next = f current
            if iteration >= maxIterations then None
            elif abs (next - current) < tolerance then Some next
            else loop next (iteration + 1)
        loop x 0