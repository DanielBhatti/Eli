namespace Eli.Math.Fs

module Function =
    let rec factorial n = n * factorial (n - 1)

    let heaviside (x: float) = if x < 0.0 then 0.0 else 1.0

    let kronecker i j = if i = j then 1 else 0

    let gaussian (mu: float) (sigma: float) (x: float) = exp(- (x - mu) * (x - mu) / (2.0 * sigma * sigma)) / (sqrt(2.0 * System.Math.PI) * sigma)