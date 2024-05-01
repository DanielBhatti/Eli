namespace Eli.Math.Fs

module NumericalAnalysis =
    let rec integrate methodStep f' tStart xStart tEnd h =
        if tStart >= tEnd then xStart
        else integrate methodStep f' (tStart + h) (methodStep f' tStart xStart h) tEnd h

    let rk4Step f' t x h =
        let k1 = f' t x
        let k2 = f' (t + h / 2.0) (x + h / 2.0 * k1)
        let k3 = f' (t + h / 2.0) (x + h / 2.0 * k2)
        let k4 = f' (t + h) (x + h * k3)
        x + h / 6.0 * (k1 + 2.0 * k2 + 2.0 * k3 + k4)

    let rec rk4Integrate f' tStart xStart tEnd h = integrate rk4Step f' tStart xStart tEnd h

    let eulerStep f' (x: float) y h = y + h * f' x y

    let rec eulerIntegrate f' tStart xStart tEnd h = integrate eulerStep f' tStart xStart tEnd h

    let heunStep f' t x h =
        let yEuler = eulerStep f' t x h
        let yCorrected = x + h / 2.0 * (f' t x + f' (t + h) yEuler)
        yCorrected

    let rec heunIntegrate f' tStart xStart tEnd h = integrate heunStep f' tStart xStart tEnd h
    
    let midpointStep f' t x h =
        let midPoint = x + h / 2.0 * f' t x
        x + h * f' (t + h / 2.0) midPoint

    let rec midpointIntegrate f' tStart xStart tEnd h = integrate midpointStep f' tStart xStart tEnd h

    let rec tracePath methodStep f' (tStart: float) xStart tEnd h path =
        if tStart >= tEnd then
            List.rev path
        else
            let xNext = tStart + h
            let yNext = methodStep f' tStart xStart h
            tracePath methodStep f' xNext yNext tEnd h ((xNext, yNext)::path)
