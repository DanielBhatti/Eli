namespace Eli.Math.Fs.Test

open NUnit.Framework
open NUnit.Framework.Legacy
open Eli.Math.Fs

module DerivativeTest =

    [<Test>]
    let ``Test forward derivative`` () =
        let f x = x * x
        let result = Derivative.derivative f 2.0 0.001 Derivative.Method.Forward
        let expected = 4.001
        ClassicAssert.AreEqual(expected, result, 0.001)

    [<Test>]
    let ``Test backward derivative`` () =
        let f x = x * x
        let result = Derivative.derivative f 2.0 0.001 Derivative.Method.Backward
        let expected = 3.999
        ClassicAssert.AreEqual(expected, result, 0.001)

    [<Test>]
    let ``Test central derivative`` () =
        let f x = x * x
        let result = Derivative.derivative f 2.0 0.001 Derivative.Method.Central
        let expected = 4.0
        ClassicAssert.AreEqual(expected, result, 0.001)

    [<Test>]
    let ``Test nth derivative`` () =
        let f x = x * x * x
        let result = Derivative.nthDerivative f 2.0 2 0.001 Derivative.Method.Central
        let expected = 12.0
        ClassicAssert.AreEqual(expected, result, 0.01)

    [<Test>]
    let ``Test gradient`` () =
        let f (x: float[]) = x.[0] * x.[0] + x.[1] * x.[1]
        let point = [| 3.0; 4.0 |]
        let result = Derivative.gradient f point 0.00001 Derivative.Method.Central
        let expected = [| 6.0; 8.0 |]
        for i in 0..result.Length - 1 do
            ClassicAssert.AreEqual(expected.[i], result.[i], 0.01)

    [<Test>]
    let ``Test jacobian`` () =
        let f (x: float[]) = [| x.[0] * x.[0]; x.[1] * x.[1] |]
        let point = [| 3.0; 4.0 |]
        let result = Derivative.jacobian f point 0.00001
        let expected = [| [| 6.0; 0.0 |]; [| 0.0; 8.0 |] |]
        for i in 0..result.Length - 1 do
            for j in 0..result.[i].Length - 1 do
                ClassicAssert.AreEqual(expected.[i].[j], result.[i].[j], 0.01)