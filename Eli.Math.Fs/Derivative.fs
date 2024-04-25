namespace Eli.Math.Fs

module Derivative =
    let derivative f x delta = (f (x + delta) - f (x - delta)) / (2 * delta)
    
    let partialDerivative f xs i delta =
        let xsCopy = Array.copy xs
        let originalXi = xsCopy.[i]
        xsCopy.[i] <- originalXi + delta
        let fPlusDelta = f xsCopy
        xsCopy.[i] <- originalXi
        let fOriginal = f xsCopy
        (fPlusDelta - fOriginal) / delta
