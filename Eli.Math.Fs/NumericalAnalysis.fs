namespace Eli.Math.Fs

module NumericalAnalysis =
    let rk4Step f' x y h =
        let k1 = f' x y
        let k2 = f' (x + h / 2.0) (y + h / 2.0 * k1)
        let k3 = f' (x + h / 2.0) (y + h / 2.0 * k2)
        let k4 = f' (x + h) (y + h * k3)

        y + h / 6.0 * (k1 + 2.0 * k2 + 2.0 * k3 + k4)

    let rk4Integrate f' xStart yStart xEnd h =
        let mutable x = xStart
        let mutable y = yStart
        while x < xEnd do
            y <- rk4Step f' h x y
            x <- x + h
        y
