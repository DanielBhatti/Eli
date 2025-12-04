namespace Eli.Math.Fs.Test

open NUnit.Framework

module DynamicsTest =
    open NUnit.Framework.Legacy
    open Eli.Math.Fs

    [<Test>]
    let ``Test convergence on a simple linear function`` () =
        let f x = x / 2.0 + 1.0
        match Dynamics.fixedPoint f 0.0 0.001 100 with
        | Some result -> ClassicAssert.AreEqual(2.0, result, 0.001)
        | None -> Assert.Fail("Did not converge")

    [<Test>]
    let ``Test non-convergence on a divergent function`` () =
        let f x = 2.0 * x          
        match Dynamics.fixedPoint f 1.0 0.001 10 with
        | Some result -> Assert.Fail("Unexpected convergence")
        | None -> Assert.Pass("Correctly identified non-convergence")

    [<Test>]
    let ``Test oscillation behavior`` () =
        let f x = -x          
        match Dynamics.fixedPoint f 1.0 0.001 100 with
        | Some result -> Assert.Fail("Unexpected convergence")
        | None -> Assert.Pass("Correctly identified non-convergence")

    [<Test>]
    let ``Test function with multiple potential fixed points`` () =
        let f x = cos x          
        match Dynamics.fixedPoint f 0.5 0.0001 1000 with
        | Some result -> ClassicAssert.IsTrue(abs(result - 0.739085) < 0.001)
        | None -> Assert.Fail("Did not converge")

    [<Test>]
    let ``Test tolerance sensitivity`` () =
        let f x = x / 2.0 + 1.0
        match Dynamics.fixedPoint f 0.0 0.1 100 with          
        | Some result -> ClassicAssert.AreEqual(2.0, result, 0.1)
        | None -> Assert.Fail("Did not converge")
