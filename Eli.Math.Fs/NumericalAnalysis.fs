namespace Eli.Math.Fs

module NumericalAnalysis =
    let private rk4Step f' x y h =
        let k1 = f' x y
        let k2 = f' (x + h / 2.0) (y + h / 2.0 * k1)
        let k3 = f' (x + h / 2.0) (y + h / 2.0 * k2)
        let k4 = f' (x + h) (y + h * k3)
        y + h / 6.0 * (k1 + 2.0 * k2 + 2.0 * k3 + k4)

    let rk4Integrate f' xStart yStart xEnd h =
        let mutable x = xStart
        let mutable y = yStart
        while x < xEnd do
            y <- rk4Step f' x y h
            x <- x + h
        y

    let private eulerStep f' x y h = y + h * f' x y

    let eulerIntegrate f' xStart yStart xEnd h =
        let mutable x = xStart
        let mutable y = yStart
        while x < xEnd do
            y <- eulerStep f' x y h
            x <- x + h
        y

    let private heunStep f' x y h =
        let yEuler = y + h * f' x y
        let yCorrected = y + h / 2.0 * (f' x y + f' (x + h) yEuler)
        yCorrected

    let heunIntegrate f' xStart yStart xEnd h =
        let mutable x = xStart
        let mutable y = yStart
        while x < xEnd do
            y <- heunStep f' x y h
            x <- x + h
        y

    let private midpointStep f' x y h =
        let midPoint = y + h / 2.0 * f' x y
        y + h * f' (x + h / 2.0) midPoint

    let midpointIntegrate f' xStart yStart xEnd h =
        let mutable x = xStart
        let mutable y = yStart
        while x < xEnd do
            y <- midpointStep f' x y h
            x <- x + h
        y
