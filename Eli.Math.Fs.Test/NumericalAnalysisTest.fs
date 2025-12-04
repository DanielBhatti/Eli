namespace Eli.Math.Fs.Test

open NUnit.Framework
open NUnit.Framework.Legacy
open Eli.Math.Fs
open System

module NumericalAnalysisTest =
    let tolerance = 1e-5

    [<Test>]
    let ``Test rk4Integrate with exponential growth``() =
        let f x y = y
        let xStart = 0.0
        let yStart = 1.0
        let xEnd = 1.0
        let h = 1e-6
        let result = Integration.rk4Integrate f xStart yStart xEnd h
        let expected = Math.Exp(xEnd)
        ClassicAssert.AreEqual(expected, result, tolerance)

    [<Test>]
    let ``Test rk4Integrate with small step size``() =
        let f x y = y
        let xStart = 0.0
        let yStart = 1.0
        let xEnd = 1.0
        let h = 1e-8
        let result = Integration.rk4Integrate f xStart yStart xEnd h
        let expected = Math.Exp(xEnd)
        ClassicAssert.AreEqual(expected, result, tolerance)

    [<Test>]
    let ``Test rk4Integrate on a known polynomial solution``() =
        let f x y = 3.0 * x * x
        let xStart = 0.0
        let yStart = 0.0
        let xEnd = 1.0
        let h = 1e-6
        let result = Integration.rk4Integrate f xStart yStart xEnd h
        let expected = xEnd ** 3.0
        ClassicAssert.AreEqual(expected, result, tolerance)

    [<Test>]
    let ``Test eulerIntegrate with exponential growth``() =
        let f x y = y
        let xStart = 0.0
        let yStart = 1.0
        let xEnd = 1.0
        let h = 1e-6
        let result = Integration.eulerIntegrate f xStart yStart xEnd h
        let expected = Math.Exp(xEnd)
        ClassicAssert.AreEqual(expected, result, tolerance)

    [<Test>]
    let ``Test heunIntegrate with exponential growth``() =
        let f x y = y
        let xStart = 0.0
        let yStart = 1.0
        let xEnd = 1.0
        let h = 1e-6
        let result = Integration.heunIntegrate f xStart yStart xEnd h
        let expected = Math.Exp(xEnd)
        ClassicAssert.AreEqual(expected, result, tolerance)

    [<Test>]
    let ``Test midpointIntegrate with exponential growth``() =
        let f x y = y
        let xStart = 0.0
        let yStart = 1.0
        let xEnd = 1.0
        let h = 1e-6
        let result = Integration.midpointIntegrate f xStart yStart xEnd h
        let expected = Math.Exp(xEnd)
        ClassicAssert.AreEqual(expected, result, tolerance)
