// return an integer exit code
module Program

open System
open System.IO
open System.Text.RegularExpressions

module Program =
    open System.Linq

    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
        else None
        
    let matchFirstNumberInString =
        function 
        | Regex @".*\d.*" [result] ->
            match Int32.TryParse(result) with 
            | (true, int) -> Some(int)
            | _ -> None
        | _ ->
            None
        
    let readFilenamesInDirectory (directory : DirectoryInfo) pattern =
        match directory.Exists with
        | false -> Seq.empty
        | true -> directory.EnumerateFiles(pattern) |> Seq.map (fun f -> f.Name)
    
    [<EntryPoint>]
    let main argv =
        let currentDirectory = new DirectoryInfo(Environment.CurrentDirectory)
        let min, max ,count = 
            readFilenamesInDirectory currentDirectory "*.*" 
            |> Seq.choose matchFirstNumberInString
            |> Seq.sort
            |> fun x -> Seq.min x, Seq.max x, Seq.length x
        printfn "Found range %i to %i with %i members" min max count
        printfn "All files enumerated"
        0
