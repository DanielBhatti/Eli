namespace Eli.Math.Fs

module NumericalAnalysis =
    let rk4Step f' x y h =
        let k1 = f' x y
        let k2 = f' (x + h / 2.0) (y + h / 2.0 * k1)
        let k3 = f' (x + h / 2.0) (y + h / 2.0 * k2)
        let k4 = f' (x + h) (y + h * k3)
        y + h / 6.0 * (k1 + 2.0 * k2 + 2.0 * k3 + k4)

    let rec rk4Integrate f' xStart yStart xEnd h =
        if xStart >= xEnd then yStart
        else rk4Integrate f' (xStart + h) (rk4Step f' xStart yStart h) xEnd h

    let eulerStep f' (x: float) y h = y + h * f' x y

    let rec eulerIntegrate f' xStart yStart xEnd h =
        if xStart >= xEnd then yStart
        else eulerIntegrate f' (xStart + h) (eulerStep f' xStart yStart h) xEnd h

    let heunStep f' x y h =
        let yEuler = eulerStep f' x y h
        let yCorrected = y + h / 2.0 * (f' x y + f' (x + h) yEuler)
        yCorrected

    let rec heunIntegrate f' xStart yStart xEnd h =
        if xStart >= xEnd then yStart
        else heunIntegrate f' (xStart + h) (heunStep f' xStart yStart h) xEnd h

    let midpointStep f' x y h =
        let midPoint = y + h / 2.0 * f' x y
        y + h * f' (x + h / 2.0) midPoint

    let rec midpointIntegrate f' xStart yStart xEnd h =
        if xStart >= xEnd then yStart
        else midpointIntegrate f' (xStart + h) (midpointStep f' xStart yStart h) xEnd h

    let rec tracePath f' (xStart: float) yStart xEnd h path method =
        if xStart >= xEnd then
            List.rev path
        else
            let xNext = xStart + h
            let yNext = method f' xStart yStart h
            tracePath f' xNext yNext xEnd h ((xNext, yNext)::path) method
