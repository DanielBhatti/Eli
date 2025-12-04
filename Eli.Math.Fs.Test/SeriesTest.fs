namespace Eli.Math.Fs.Test

open NUnit.Framework
open NUnit.Framework.Legacy
open Eli.Math.Fs

[<TestFixture>]
module SeriesTest =
    [<Test>]
    let ``Test series expansion for a linear function``() =
        let f x = 2.0 * x + 1.0
        let x0 = 0.0
        let n = 3
        let h = 1e-5
        let series = Series.taylorExpansion f x0 n h
        ClassicAssert.AreEqual(f 1.0, series 1.0)

    [<Test>]
    let ``Test series expansion for a quadratic function``() =
        let f x = 2.0 * x * x + 3.0 * x + 4.0
        let x0 = 1.0
        let n = 3
        let h = 1e-5
        let series = Series.taylorExpansion f x0 n h
        ClassicAssert.AreEqual(f 2.0, series 2.0)

    [<Test>]
    let ``Test series expansion for a cubic function``() =
        let f x = x * x * x
        let x0 = 1.0
        let n = 4
        let h = 1e-5
        let series = Series.taylorExpansion f x0 n h
        ClassicAssert.AreEqual(f 2.0, series 2.0)
