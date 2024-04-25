namespace Eli.Math.Fs

module MetaFunction =
    open System.Collections.Generic

    let rec nest n f x =
        match n with
        | n when n > 0 -> nest (n - 1) f (f x)
        | _ -> x

    let iterate n f x =
        let rec loop i accumulate =
            if i >= n then accumulate
            else loop (i + 1) (f accumulate :: accumulate)
        loop 1 [x] |> List.rev

    let memoize f =
        let m = Dictionary<_, _>()  
        let rec fRec x =
            let mutable result = Unchecked.defaultof<_>
            if m.TryGetValue(x, &result) then result  
            else
                let computed = f fRec x  
                m.Add(x, computed)  
                computed  
        fRec