namespace Eli.Math.Fs.Test

open NUnit.Framework
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
        let h = 0.1
        let result = NumericalAnalysis.rk4Integrate f xStart yStart xEnd h
        let expected = Math.Exp(xEnd)
        Assert.AreEqual(expected, result, tolerance)

    [<Test>]
    let ``Test rk4Integrate with small step size``() =
        let f x y = y
        let xStart = 0.0
        let yStart = 1.0
        let xEnd = 1.0
        let h = 0.0001
        let result = NumericalAnalysis.rk4Integrate f xStart yStart xEnd h
        let expected = Math.Exp(xEnd)
        Assert.AreEqual(expected, result, tolerance)

    [<Test>]
    let ``Test rk4Integrate on a known polynomial solution``() =
        let f x y = 3.0 * x * x
        let xStart = 0.0
        let yStart = 0.0
        let xEnd = 1.0
        let h = 0.1
        let result = NumericalAnalysis.rk4Integrate f xStart yStart xEnd h
        let expected = xEnd ** 3.0
        Assert.AreEqual(expected, result, tolerance)
