namespace Eli.Math.Fs

module Sequence =
    let _fibonacci _fibonacci = function
        | 0 | 1 as n -> n
        | n -> _fibonacci(n - 1) + _fibonacci(n - 2)
    let fibonacci = MetaFunction.memoize _fibonacci

    let _factorial _factorial = function
        | 0 | 1 as n -> n
        | n -> n * _factorial(n - 1)
    let factorial = MetaFunction.memoize _factorial