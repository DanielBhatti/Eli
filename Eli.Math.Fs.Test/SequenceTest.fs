namespace Eli.Math.Fs.Test

open NUnit.Framework

module SequenceTest =
    open NUnit.Framework.Legacy
    open Eli.Math.Fs

    [<Test>]
    let ``Test fibonacci sequence`` () =
        let fibonacciResults = [0; 1; 1; 2; 3; 5; 8; 13; 21; 34; 55]
        let testResults = List.indexed fibonacciResults |> List.map (fun (i, expected) -> (i, Sequence.fibonacci i, expected))
        let errors = testResults |> List.filter (fun (i, actual, expected) -> actual <> expected)
        
        if errors <> [] then
            let message = 
                errors 
                |> List.map (fun (i, actual, expected) -> sprintf "Fib(%d) = %d but expected %d" i actual expected)
                |> String.concat ", "
            Assert.Fail(message)
